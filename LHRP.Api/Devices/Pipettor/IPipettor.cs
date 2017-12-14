using LHRP.Api.Runtime;

namespace LHRP.Api.Devices.Pipettor
{
    public interface IPipettor : IDevice
    {
        void Aspirate(AspirateParameters parameters, ICommandExecutor commandExecutor);
        void Dispense(DispenseParameters parameters, ICommandExecutor commandExecutor);
        void PickupTips(TipPickupParameters parameters, ICommandExecutor commandExecutor);
        void DropTips(TipDropParameters parameters, ICommandExecutor commandExecutor);
    }
}