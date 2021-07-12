using LHRP.Api.Runtime.ErrorHandling;

namespace LHRP.Api.Runtime.Compilation
{
    public class CompileDefaultErrorResolver : IErrorResolver
    {
        public ProcessResult Resolve<TErrorType>(IRuntimeEngine engine, TErrorType error) where TErrorType : RuntimeError
        {
            var process = new ProcessResult();
            //When compiling, we just collect all the errors, so assume errors get resolved
            return process;
        }
    }
}
