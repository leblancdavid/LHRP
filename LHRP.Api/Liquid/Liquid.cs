using System;

namespace LHRP.Api.Liquid
{
    public class Liquid
    {
        public Guid UniqueId { get; private set; }
        public double Volume { get; private set; }
        private string _assignedId;
        public string AssignedId { get; private set; }
        public LiquidDefinition LiquidDefinition { get; private set; }

        public Liquid(LiquidDefinition definition, double initialVolume)
        {
            LiquidDefinition = definition;
            Volume = initialVolume;
            _assignedId = "";
            UniqueId = Guid.NewGuid();
        }

        public void AddVolume(double volume)
        {
            Volume += volume;
        }

        public void RemoveVolume(double volume)
        {
            Volume -= volume;
        }
    }
}