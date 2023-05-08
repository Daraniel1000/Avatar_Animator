using Assets.Scenes.FaceTracking;
using MessagePack;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [MessagePackObject(true)]
    public class BodyData
    {
        public FaceMediapipeData Face { get; set; }
        public List<Vec3> Body { get; set; }
        public List<Vec3> HandL { get; set; }
        public List<Vec3> HandR { get; set; }
    }

    [MessagePackObject(true)]
    public class FaceMediapipeData
    {
        public Vec3 TopLeft { get; set; }
        public Vec3 TopRight { get; set; }
        public Vec3 BottomLeft { get; set; }
        public Vec3 BottomRight { get; set; }
    }
}
