namespace kbs2.Actions
{
    public interface IActionModel
    {
        float CoolDown { get; }
        float CurrentCoolDown { get; set; }
    }
}
