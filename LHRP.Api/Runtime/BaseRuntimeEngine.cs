using LHRP.Api.Instrument;
using LHRP.Api.Runtime.Errors;

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
                var result = Commands.RunNextCommand();
                if(result.ContainsErrors)
                {
                    foreach(var error in result.Errors)
                    {
                        var resolver = ErrorHandler.HandleError(error);
                        resolver.Resolve(Commands);
                    }
                }

                process.AppendSubProcess(result);
            }

            return process;
        }
    }
}