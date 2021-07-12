using LHRP.Api.Runtime.Compilation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Domain.Tests.Runtime.Compilation
{
    public class CompilationTests
    {
        ICompilationEngine engine;
        public CompilationTests()
        {
            engine = RuntimeEngineDataProvider.BuildSimulationRunEngine(4).GetCompilationEngine();
        }


    }
}
