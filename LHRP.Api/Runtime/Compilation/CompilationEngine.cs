using LHRP.Api.Instrument;
using LHRP.Api.Runtime.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Runtime.Compilation
{
    public class CompilationEngine : ICompilationEngine
    {
        public IInstrument Instrument => throw new NotImplementedException();

        public IRuntimeCommandQueue Commands => throw new NotImplementedException();

        public IErrorHandler ErrorHandler => throw new NotImplementedException();

        public RuntimeStatus Status => throw new NotImplementedException();

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public ICompilationEngine GetCompilationEngine()
        {
            return this;
        }

        public IRuntimeEngine GetSnapshot()
        {
            throw new NotImplementedException();
        }

        public ProcessResult Run()
        {
            throw new NotImplementedException();
        }
    }
}
