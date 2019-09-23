using LHRP.Api.Instrument;
using LHRP.Api.Runtime.ErrorHandling;
using LHRP.Api.Runtime.ErrorHandling.Errors;

namespace LHRP.Api.Runtime
{
    public class BaseRuntimeEngine : IRuntimeEngine
    {
        public BaseRuntimeEngine(IInstrument instrument, IRuntimeCommandQueue commands, IErrorHandler errorHandler)
        {
            Instrument = instrument;
            Commands = commands;
            ErrorHandler = errorHandler;
            Status = RuntimeStatus.Idle;
        }
        public IInstrument Instrument { get; protected set; }

        public IRuntimeCommandQueue Commands { get; protected set; }
        public IErrorHandler ErrorHandler { get; protected set; }
        public RuntimeStatus Status { get; protected set; }

        public void Abort()
        {
            Commands.Clear();
            Status = RuntimeStatus.Aborted;
        }

        public ProcessResult Run()
        {
            var process = new ProcessResult();
            while(!Commands.IsCompleted && Status != RuntimeStatus.Aborted)
            {
                var result = Commands.RunNextCommand(this);
                if(result.ContainsErrors)
                {
                    foreach(var error in result.Errors)
                    {
                        var errorHandlingResult = ErrorHandler.HandleError(this, error);
                        if(errorHandlingResult.IsFailure)
                        {
                            Commands.Clear();
                            process.AddError(new RuntimeError(errorHandlingResult.Error));
                        }
                    }
                }

                process.AppendSubProcess(result);
            }

            return process;
        }
    }
}