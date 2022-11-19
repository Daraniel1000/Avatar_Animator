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

        private List<VertexObject> vertexBones { get; set; }

        private Vector3 faceScale;

        private static readonly string[][] boneNames = {
            new[] { "Mouth.R" }, new[] { "Mouth.T.R" }, new[] { "Mouth.T.Center" }, new[] { "Mouth.T.L" }, 
            new[] { "Mouth.L" }, new[] { "Mouth.B.L" }, new[] { "Mouth.B.Center" }, new[] { "Mouth.B.R" } };

        public FaceHelper(GameObject root)
        {
            faceScale = root.transform.lossyScale;
            vertexBones = new List<VertexObject>();
            foreach(string[] bones in boneNames)
            {
                VertexObject current = new VertexObject();
                foreach (string bone in bones)
                {
                    GameObject boneObject = GameObject.Find(bone);
                    if (boneObject == null) Debug.LogError($"{bone} object not found");
                    current.Bones.Add(boneObject);
                }
                vertexBones.Add(current);
            }
        }

        public void HandleFaceUpdate(DataStreamReader stream)
        {
            Vector3 currentVertex = new Vector3();
            foreach (var vertexObject in vertexBones)
            {
                currentVertex.x = stream.ReadFloat();
                currentVertex.y = stream.ReadFloat();
                currentVertex.z = stream.ReadFloat();
                foreach (GameObject bone in vertexObject.Bones)
                {
                    bone.transform.localPosition = currentVertex.divide(faceScale);
                }
            }
        }
    }
}
