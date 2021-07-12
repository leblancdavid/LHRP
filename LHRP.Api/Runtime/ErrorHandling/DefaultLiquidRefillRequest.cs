using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using LHRP.Api.Liquids;
using System.Text;

namespace LHRP.Api.Runtime.ErrorHandling
{
    public class DefaultLiquidRefillRequest : IErrorResolver
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

            var containers = engine.Instrument.Deck.GetLiquidContainers()
                .Where(x => x.ContainsLiquid(insuffientLiquidError.RequestedLiquid)).ToList();

            if (!containers.Any())
            {
                process.AddError(new RuntimeError($"No containers have been assigned to liquid {insuffientLiquidError.RequestedLiquid.GetId()}"));
                return process;
            }

            double volume = insuffientLiquidError.RemainingVolumeNeeded;
            int containerIndex = 0;
            while (containerIndex < containers.Count())
            {
                var c = containers[containerIndex];
                if (c.MaxVolume < volume)
                {
                    volume -= c.MaxVolume;
                    c.AddLiquid(insuffientLiquidError.RequestedLiquid, c.MaxVolume);
                }
                else
                {
                    c.AddLiquid(insuffientLiquidError.RequestedLiquid, volume);
                    break;
                }
                containerIndex++;
            }

            engine.Commands.MoveToLastExecutedCommand();

            return process;
        }
    }
}
