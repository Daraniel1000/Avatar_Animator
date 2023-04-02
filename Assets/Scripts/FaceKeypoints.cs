using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class FaceKeypoints
    {
        public List<Vector3> vertices { get; set; } = new List<Vector3>();
        public Vector3 pos { get; set; }
        public Quaternion rot { get; set; }
    }
}
