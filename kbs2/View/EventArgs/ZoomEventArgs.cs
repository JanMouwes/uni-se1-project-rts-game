namespace kbs2.Desktop.View.EventArgs
{
    public class ZoomEventArgs : System.EventArgs
    {
        public double Zoom { get; }

        public ZoomEventArgs(double zoom)
        {
            Zoom = zoom;
        }
    }
}