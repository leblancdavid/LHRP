using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public interface IPipettor : IDevice
    {
        void Aspirate(AspirateParameters parameters);
        void Dispense(DispenseParameters parameters);
        void PickupTips(TipPickupParameters parameters);
        void DropTips(TipDropParameters parameters);
    }
}