using System;

namespace LHRP.Api.Liquids
{
    public class LiquidPart : Liquid
    {
        public const int DEFAULT_CONCENTRATION_PRECISION = 3;   
        public double Concentration { get; set; }
        public int ConcentrationPrecision { get; private set; }
        public LiquidPart()
        {
            Concentration = 1.0;
            ConcentrationPrecision = DEFAULT_CONCENTRATION_PRECISION;
        }

        public LiquidPart(string name, double concentration = 1.0, int precision = DEFAULT_CONCENTRATION_PRECISION)
        {
            _assignedId = name;
            Concentration = concentration;
            ConcentrationPrecision = precision;
        }

        

        public override string GetId()
        {
            return $"{_assignedId}({Math.Round(Concentration, ConcentrationPrecision)})";
        }

        public override string ToString()
        {
            return GetId();
        }
    }
}
