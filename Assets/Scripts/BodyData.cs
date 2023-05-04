using MessagePack;

namespace Assets.Scripts
{
    [MessagePackObject(true)]
    public class BodyData
    {
        public int fps { get; set; }
    }
}
