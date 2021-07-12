using LHRP.Api.Instrument;
using LHRP.Api.Runtime.Compilation;
using LHRP.Api.Runtime.ErrorHandling;

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

        public virtual ICompilationEngine GetCompilationEngine()
        {
            return new CompilationEngine(Instrument, Commands);
        }

        public virtual IRuntimeEngine GetSnapshot()
        {
            return new BaseRuntimeEngine(Instrument.GetSnapshot(), Commands.GetSnapshot(), ErrorHandler);
        }

        public virtual ProcessResult Run()
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