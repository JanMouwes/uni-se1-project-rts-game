namespace kbs2.WorldEntity.Interfaces
{
    public interface ITrainable : ISpawnable
    {
        ITrainableDef Def { get; }
    }
}