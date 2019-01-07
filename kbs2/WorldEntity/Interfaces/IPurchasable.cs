namespace kbs2.Unit.Interfaces
{
    public interface IPurchasable
    {
        /// <summary>
        /// Amount it costs to purchase
        /// </summary>
        double Cost { get; set; }
        
        /// <summary>
        /// Amount it costs to maintain per X time
        /// </summary>
        double UpkeepCost { get; set; }
    }
}
