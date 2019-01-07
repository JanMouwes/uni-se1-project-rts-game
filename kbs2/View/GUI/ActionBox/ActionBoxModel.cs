using kbs2.World.Structs;

namespace kbs2.View.GUI.ActionBox
{
    public class ActionBoxModel
    {
        public ActionBoxTextView Text { get; set; }
        public bool Show { get; set; }

        public ActionBoxModel(FloatCoords loc)
        {
            Text = new ActionBoxTextView(loc);
            Show = false;
        }
    }
}
