using System;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

namespace Assets.Scripts
{
    public class FaceHelper
    {
        private class VertexObject
        {
            public List<GameObject> Bones = new List<GameObject>();
            public Vector3 originalPos;
        }

        private List<GameObject> vertexBones { get; set; }

        public float faceScale;

        private static readonly string[] boneNames = {
            "Mouth.R","Mouth.T.R", "Mouth.T.Center", "Mouth.T.L", 
            "Mouth.L", "Mouth.B.L", "Mouth.B.Center", "Mouth.B.R",
            "Mouth.T.R.001", "Mouth.T.R.002", "Mouth.T.L.001",
            "Mouth.T.L.002", "Mouth.B.R.001", "Mouth.B.R.002",
            "Mouth.B.L.001", "Mouth.B.L.002", "Jaw", "Jaw.R", "Jaw.L",
            "Jaw.R.001", "Jaw.L.001", "Cheek.R", "Cheek.R.001",
            "Cheek.R.002", "Cheek.L", "Cheek.L.001", "Cheek.L.002"};

        public FaceHelper(GameObject root)
        {
            faceScale = root.transform.lossyScale.x;
            vertexBones = new List<GameObject>();
            foreach(string bone in boneNames)
            {
                GameObject boneObject = GameObject.Find(bone);
                if (boneObject == null) Debug.LogError($"{bone} object not found");
                vertexBones.Add(boneObject);
            }
        }

        public void HandleFaceUpdate(DataStreamReader stream)
        {
            int i = 0;
            Vector3 currentVertex = new Vector3();
            foreach (var bone in vertexBones)
            {
                currentVertex.x = stream.ReadFloat();
                currentVertex.y = stream.ReadFloat();
                currentVertex.z = stream.ReadFloat();
                bone.transform.localPosition = currentVertex;
                ++i;
            }
        }
    }
}
