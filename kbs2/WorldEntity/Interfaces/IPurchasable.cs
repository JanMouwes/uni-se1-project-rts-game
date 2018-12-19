using System;
namespace kbs2.Unit.Interfaces
{
    public interface IPurchasable
    {
        double Cost { get; set; }
        double UpkeepCost { get; set; }
    }
}
