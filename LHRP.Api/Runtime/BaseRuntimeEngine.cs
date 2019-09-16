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
        }
        public IInstrument Instrument { get; protected set; }

        public IRuntimeCommandQueue Commands { get; protected set; }
        public IErrorHandler ErrorHandler { get; protected set; }
        public Process Run()
        {
            var process = new Process();
            while(!Commands.IsCompleted)
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