using Assets.Scenes.FaceTracking;
using MessagePack;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [MessagePackObject(true)]
    public class BodyData
    {
        public List<Vec3> Body { get; set; }
        public HandsMediapipeData Hands { get; set; }
    }

    [MessagePackObject(true)]
    public class HandsMediapipeData
    {
        public List<List<Vec3>> Landmarks { get; set; }
        public List<MultiHandednessData> MultiHandedness { get; set;}
    }

    [MessagePackObject(true)]
    public class MultiHandednessData
    {
        public int index { get; set; }
        public string label { get; set; }
    }
}
