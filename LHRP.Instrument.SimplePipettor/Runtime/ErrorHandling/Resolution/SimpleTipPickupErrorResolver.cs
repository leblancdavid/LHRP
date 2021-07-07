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
        public Result Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var insuffientLiquidError = error as InsufficientLiquidRuntimeError;
            if (insuffientLiquidError == null)
            {
                return Result.Failure($"Invalid error type {error.GetType()}");
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
                return Result.Ok();
            }

        }

        private Result TryRefillAndContinuePipetteSequence(IRuntimeEngine engine, InsufficientLiquidRuntimeError error)
        {
            var containers = engine.Instrument.Deck.GetLiquidContainers()
                 .Where(x => x.ContainsLiquid(error.RequestedLiquid)).ToList();

            if (!containers.Any())
            {
                return Result.Failure($"No containers have been assigned to liquid {error.RequestedLiquid.GetId()}");
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

            return Result.Ok();
        }
    }
}
