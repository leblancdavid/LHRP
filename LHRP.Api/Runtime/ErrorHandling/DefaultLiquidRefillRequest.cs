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
        public Result Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var insuffientLiquidError = error as InsufficientLiquidRuntimeError;
            if (insuffientLiquidError == null)
            {
                return Result.Failure($"Invalid error type {error.GetType()}");
            }

            var containers = engine.Instrument.Deck.GetLiquidContainers()
                .Where(x => x.ContainsLiquid(insuffientLiquidError.RequestedLiquid)).ToList();

            if (!containers.Any())
            {
                return Result.Failure($"No containers have been assigned to liquid {insuffientLiquidError.RequestedLiquid.AssignedId}");
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

            return Result.Ok();
        }
    }
}
