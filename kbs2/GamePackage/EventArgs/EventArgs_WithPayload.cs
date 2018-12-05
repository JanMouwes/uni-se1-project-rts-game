using kbs2.GamePackage;

namespace kbs2.GamePackage.EventArgs
{
    public class EventArgs_WithPayload<PayloadType> : System.EventArgs
    {
        public PayloadType payload { get; }

        public EventArgs_WithPayload(PayloadType payload)
        {
            this.payload = payload;
        }
    }
}