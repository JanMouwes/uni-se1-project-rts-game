using kbs2.GamePackage.AIPackage.Enums;
using kbs2.Unit.Interfaces;

namespace kbs2.WorldEntity.Unit.MVC
{
    public interface IHasCommand
    {
        Command Order { get; set; }
        IHasPersonalSpace Target { get; set; }
        bool FinishedOrder { get; set; }
    }
}