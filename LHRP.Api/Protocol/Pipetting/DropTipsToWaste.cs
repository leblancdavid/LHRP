using CSharpFunctionalExtensions;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;

namespace LHRP.Api.Protocol.Pipetting
{
    public class DropTipsToWaste : IRunnable
    {
        public DropTipsToWaste()
        {

        }

        public Result<Process> Run(IInstrument instrument)
        {
            var process = new Process();
            var pipettor = instrument.GetPipettor();

            //pipettor.DropTips()
            //process.AppendSubProcess(commandResult.Value);

            return Result.Ok<Process>(process);
        }
    }
}