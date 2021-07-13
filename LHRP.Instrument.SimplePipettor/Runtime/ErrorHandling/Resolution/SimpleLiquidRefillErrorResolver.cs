using CSharpFunctionalExtensions;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.ErrorHandling;
using System;
using System.Linq;

namespace LHRP.Instrument.SimplePipettor.Runtime.ErrorHandling
{
    public class SimpleLiquidRefillErrorResolver : IErrorResolver
    {
        public ProcessResult Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var process = new ProcessResult();
            var insuffientLiquidError = error as InsufficientLiquidRuntimeError;
            if (insuffientLiquidError == null)
            {
                var errorMsg = $"Invalid error type {error.GetType()}";
                process.AddError(new RuntimeError(errorMsg));
                return process;
            }

            Console.WriteLine(insuffientLiquidError.Message);
            
            Console.Write("Handling options: Refill and Continue (c), Abort (a): ");
            var requestInput = Console.ReadLine();
            if(requestInput == "c")
            {
                Console.WriteLine("Continuing...");
                return TryRefillAndContinuePipetteSequence(engine, insuffientLiquidError);
            }
            else
            {
                Console.WriteLine("Aborting...");
                engine.Abort();
                return process;
            }

        }

        private ProcessResult TryRefillAndContinuePipetteSequence(IRuntimeEngine engine, InsufficientLiquidRuntimeError error)
        {
            var process = new ProcessResult();
            var containers = engine.Instrument.Deck.GetLiquidContainers()
                 .Where(x => x.ContainsLiquid(error.RequestedLiquid)).ToList();

            if (!containers.Any())
            {
                process.AddError(new RuntimeError($"No containers have been assigned to liquid {error.RequestedLiquid.GetId()}"));
                return process;
            }

            double volume = error.RemainingVolumeNeeded;
            int containerIndex = 0;
            while (containerIndex < containers.Count())
            {
                var c = containers[containerIndex];
                if (c.MaxVolume < volume)
                {
                    volume -= c.MaxVolume;
                    c.AddLiquid(error.RequestedLiquid, c.MaxVolume);
                }
                else
                {
                    c.AddLiquid(error.RequestedLiquid, volume);
                    break;
                }
                containerIndex++;
            }

            engine.Commands.MoveToLastExecutedCommand();

            return process;
        }
    }
}
