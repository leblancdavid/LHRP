using LHRP.Api.CoordinateSystem;
using LHRP.Api.Devices.Pipettor;
using LHRP.Api.Instrument;
using LHRP.Api.Runtime;
using LHRP.Api.Runtime.Compilation;
using LHRP.Domain.Tests.Instrument;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHRP.Domain.Tests.Runtime
{
    public static class RuntimeEngineDataProvider
    {
        public static IRuntimeEngine BuildSimulationRunEngine(int numPositions)
        {
            return new BaseRuntimeEngine(InstrumentDataProvider.GetSimulatedInstrument(numPositions),
                new RuntimeCommandQueue(), new CompilationErrorHandler());
        }

        
    }
}
