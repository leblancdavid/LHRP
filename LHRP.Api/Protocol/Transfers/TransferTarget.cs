using LHRP.Api.Labware;
using LHRP.Api.Liquids;

namespace LHRP.Api.Protocol.Transfers
{
  public class TransferTarget
  {
    public TransferTarget(LabwareAddress address, int positionId, Liquid liquid, double volume)
    {
      this.Address = address;
      this.PositionId = positionId;
      this.Liquid = liquid;
      this.Volume = volume;

    }
    public LabwareAddress Address { get; private set; }
    public int PositionId { get; private set; }
    public Liquid Liquid { get; private set; }
    public double Volume { get; private set; }
  }
}