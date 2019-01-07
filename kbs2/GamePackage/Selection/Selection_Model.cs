using MonoGame.Extended;

namespace kbs2.GamePackage.Selection
{
    public class Selection_Model
    {
        public RectangleF SelectionBox { get; set; }

        //constructor
        public Selection_Model()
        {
            SelectionBox = new RectangleF();
        }
    }
}
