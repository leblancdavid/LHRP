using System;

namespace LHRP.Api.Runtime.Scheduling
{
    public class ConsolePrintScheduleStream : IScheduleStream
    {
        public void Send(Schedule schedule)
        {
            Console.WriteLine("Usage in liquid containers:");
            foreach (var liquidUsage in schedule.ResourcesUsage.LiquidContainerUsages)
            {
                Console.WriteLine($"Pos{liquidUsage.Address.PositionId}-{liquidUsage.Address.ToAlphaAddress()}, Starting liquid volume: {liquidUsage.RequiredLiquidVolumeAtStart}uL, Ending volume: {liquidUsage.ExpectedFinalLiquidVolume}uL");
                foreach (var transfer in liquidUsage.TransferHistory)
                {
                    Console.WriteLine($"{transfer.TransferType} {transfer.Volume}uL");
                }
            }
            Console.WriteLine("Tip usage:");
            foreach (var tipUsage in schedule.ResourcesUsage.TipUsages)
            {
                Console.WriteLine($"Tip Type: ({tipUsage.TipTypeId}) used: {tipUsage.ExpectedTotalTipUsage}");
            }

            Console.WriteLine("Liquid usage:");
            foreach (var liquidUsage in schedule.ResourcesUsage.ConsumableLiquidUsages)
            {
                Console.WriteLine($"Liquid '{liquidUsage.Key.LiquidType.ToString()}' ({liquidUsage.Key.GetId()}): {liquidUsage.Value}uL");
            }

            Console.WriteLine($"Estimated total run-time: {schedule.ExpectedDuration.ToString("c")}");
        }
    }
}
