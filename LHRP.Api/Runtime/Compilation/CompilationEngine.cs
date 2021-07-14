using LHRP.Api.Instrument;
using LHRP.Api.Runtime.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Api.Runtime.Compilation
{
    public class CompilationEngine : BaseRuntimeEngine, ICompilationEngine
    {
        public CompilationEngine(IInstrument instrument, IRuntimeCommandQueue commands)
            : base(instrument.GetSimulation(), commands.CreateSnapshot(), new CompilationErrorHandler())
        {

        }

        public override ICompilationEngine GetCompilationEngine()
        {
            return this;
        }

        public override IRuntimeEngine CreateSnapshot()
        {
            return new CompilationEngine(Instrument.CreateSnapshot(), Commands.CreateSnapshot());
        }
    }
}
