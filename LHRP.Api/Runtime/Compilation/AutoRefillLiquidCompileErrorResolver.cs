using LHRP.Api.Runtime.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LHRP.Api.Runtime.Compilation
{
    public class AutoRefillLiquidCompileErrorResolver : IErrorResolver
    {
        public ProcessResult Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var process = new ProcessResult();
            var insuffientLiquidError = error as InsufficientLiquidRuntimeError;
            if (insuffientLiquidError == null)
            {
                process.AddError(error);
                return process;
            }

            return TryRefillAndContinuePipetteSequence(engine, insuffientLiquidError);
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
