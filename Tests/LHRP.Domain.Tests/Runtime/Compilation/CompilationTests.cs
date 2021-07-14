using FluentAssertions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Labware;
using LHRP.Api.Protocol.Pipetting;
using LHRP.Api.Protocol.Steps;
using LHRP.Api.Protocol.Transfers;
using LHRP.Api.Runtime.Compilation;
using LHRP.Domain.Tests.Labware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LHRP.Domain.Tests.Runtime.Compilation
{
    public class CompilationTests
    {
        ICompilationEngine compiler;
        public CompilationTests()
        {
            compiler = RuntimeEngineDataProvider.BuildSimulationRunEngine(4).GetCompilationEngine();
        }

        [Fact]
        public void StepShouldCompile()
        {
            var tipRack = LabwareProvider.Get300TipRack(1);
            compiler.Instrument.Deck.AddLabware(1, tipRack);
            compiler.Instrument.Deck.AddLabware(2, LabwareProvider.Get96WellPlate(2));
            compiler.Instrument.Deck.AddLabware(3, LabwareProvider.Get96WellPlate(3));

            var customStep = new CustomStep();
            customStep.AddCommand(new PickupTips(ChannelPattern.Full(compiler.Instrument.Pipettor.Specification.NumChannels), tipRack.Definition.Id));
            customStep.AddCommand(new TransferTargetAspirate(new AspirateParameters(),
                new ChannelPattern<TransferTarget>(
                    new TransferTarget[]
                    {
                        new TransferTarget(new LabwareAddress(1, 1, 2), 50, TransferType.Aspirate),
                        new TransferTarget(new LabwareAddress(1, 2, 2), 50, TransferType.Aspirate),
                    }
                )));
            customStep.AddCommand(new Dispense(new DispenseParameters(),
                new ChannelPattern<TransferTarget>(
                    new TransferTarget[]
                    {
                        new TransferTarget(new LabwareAddress(1, 1, 3), 50, TransferType.Dispense),
                        new TransferTarget(new LabwareAddress(1, 2, 3), 50, TransferType.Dispense),
                    }
                )));
            customStep.AddCommand(new DropTips());

            var compilationResult = customStep.Run(compiler);
            compilationResult.Errors.Count().Should().Be(0);


        }

        [Fact]
        public void StepShouldNotCompile()
        {
            var tipRack = LabwareProvider.Get50TipRack(1);
            compiler.Instrument.Deck.AddLabware(1, tipRack);
            compiler.Instrument.Deck.AddLabware(2, LabwareProvider.Get96WellPlate(2));
            compiler.Instrument.Deck.AddLabware(3, LabwareProvider.Get96WellPlate(3));

            var customStep = new CustomStep();
            customStep.AddCommand(new PickupTips(ChannelPattern.Full(compiler.Instrument.Pipettor.Specification.NumChannels), tipRack.Definition.Id));
            //This will not compile because we are trying to aspirate more than the tip can hold
            customStep.AddCommand(new TransferTargetAspirate(new AspirateParameters(),
                new ChannelPattern<TransferTarget>(
                    new TransferTarget[]
                    {
                        new TransferTarget(new LabwareAddress(1, 1, 2), 200, TransferType.Aspirate),
                        new TransferTarget(new LabwareAddress(1, 2, 2), 200, TransferType.Aspirate),
                    }
                )));
            customStep.AddCommand(new Dispense(new DispenseParameters(),
                new ChannelPattern<TransferTarget>(
                    new TransferTarget[]
                    {
                        new TransferTarget(new LabwareAddress(1, 1, 3), 50, TransferType.Dispense),
                        new TransferTarget(new LabwareAddress(1, 2, 3), 50, TransferType.Dispense),
                    }
                )));
            customStep.AddCommand(new DropTips());

            var compilationResult = customStep.Run(compiler);
            compilationResult.Errors.Count().Should().NotBe(0);
        }

    }
}
