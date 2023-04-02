using System;
using System.Collections.Generic;
using System.IO;
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

        private GameObject faceRoot { get; set; }

        public float faceScale;

        private List<string> boneNames = new List<string>();

        public List<int> vertexNumbers { get; set; } = new List<int>();

        public void readRigConfig()
        {
            try
            {
                StreamReader config = new StreamReader("Assets/RigVertices.txt");
                string[] s;
                boneNames.Clear();
                vertexNumbers.Clear();
                while (!config.EndOfStream)
                {
                    s = config.ReadLine().Split(' ');
                    vertexNumbers.Add(Convert.ToInt32(s[0]));
                    boneNames.Add(s[1]);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public FaceHelper()
        {
            readRigConfig();
            faceRoot = GameObject.Find("Root");
            faceScale = faceRoot.transform.lossyScale.x;
            vertexBones = new List<GameObject>();
            foreach(string bone in boneNames)
            {
                GameObject boneObject = GameObject.Find(bone);
                if (boneObject == null) Debug.LogError($"{bone} object not found");
                vertexBones.Add(boneObject);
            }
        }
        public void HandleFaceUpdate(float[] vertices)
        {
            Vector3 currentVertex = new Vector3();
            //currentVertex.x = vertices[0];
            //currentVertex.y = vertices[1];
            //currentVertex.z = vertices[2];
            //faceRoot.transform.position = currentVertex * faceScale;
            //faceRoot.transform.rotation = rotation;
            int i = 0;
            foreach (var bone in vertexBones)
            {
                currentVertex.x = vertices[i++];
                currentVertex.y = vertices[i++];
                currentVertex.z = vertices[i++];
                bone.transform.localPosition = currentVertex;
            }
        }

        public void HandleFaceUpdate(DataStreamReader stream)
        {
            Vector3 currentVertex = new Vector3();
            Quaternion rotation = new Quaternion();
            currentVertex.x = stream.ReadFloat();
            currentVertex.y = stream.ReadFloat();
            currentVertex.z = stream.ReadFloat();
            faceRoot.transform.position = currentVertex * faceScale;
            rotation.x = stream.ReadFloat();
            rotation.y = stream.ReadFloat();
            rotation.z = stream.ReadFloat();
            rotation.w = stream.ReadFloat();
            faceRoot.transform.rotation = rotation;
            if (stream.Length > 28)
            {
            int i = 0;
                foreach (var bone in vertexBones)
                {
                    currentVertex.x = stream.ReadFloat();
                    currentVertex.y = stream.ReadFloat();
                    currentVertex.z = stream.ReadFloat();
                    bone.transform.localPosition = currentVertex;
                }
            }
        }
    }
}
