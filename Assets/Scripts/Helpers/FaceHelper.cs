using Assets.Scenes.FaceTracking;
using System;
using System.Collections.Generic;
using System.IO;
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
        private GameObject faceRotator { get; set; }

        public float faceScale;
        private Quaternion baseRotation;
        private Quaternion calibrationRotation = Quaternion.identity;

        private List<string> boneNames = new List<string>();

        public List<int> vertexNumbers { get; set; } = new List<int>();

        public void readRigConfig()
        {
            try
            {
                StreamReader config = new StreamReader("Assets/RigVerticesArkit.txt");
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
            faceScale = faceRoot?.transform.lossyScale.x ?? 1;
            faceRotator = GameObject.Find("Neck IK Target");
            baseRotation = faceRotator.transform.rotation;
            vertexBones = new List<GameObject>();
            foreach(string bone in boneNames)
            {
                GameObject boneObject = GameObject.Find(bone);
                if (boneObject == null) Debug.LogError($"{bone} object not found");
                vertexBones.Add(boneObject);
            }
        }

        public void HandleFaceUpdate(FaceKeypoints keypoints)
        {
            faceRotator.transform.rotation = calibrationRotation * keypoints.rot.ToQuaternion() * baseRotation;
            if (keypoints.vertices.Count > 0)
            {
                int i = 0;
                foreach (var bone in vertexBones)
                {
                    bone.transform.localPosition = keypoints.vertices[i++].ToVector();
                }
            }
        }

        public void CalibrateFace()
        {
            calibrationRotation = Quaternion.Inverse(faceRotator.transform.rotation * Quaternion.Inverse(baseRotation) * Quaternion.Inverse(calibrationRotation));
        }
    }
}
