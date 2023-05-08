using Assets.Scenes.FaceTracking;
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
        private GameObject faceRoot1 { get; set; }

        public float faceScale;

        private List<string> boneNames = new List<string>();

        public List<int> vertexNumbers { get; set; } = new List<int>();

        private static Vector3 Nose = new Vector3(0f, -0.00011197f, -0.00003134f);
        private static Vector3[] unmodifiedVertices;
        private static float unmodifiedLen;

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
            faceRoot1 = GameObject.Find("Root.001");
            faceScale = faceRoot.transform.lossyScale.x;
            vertexBones = new List<GameObject>();
            foreach(string bone in boneNames)
            {
                GameObject boneObject = GameObject.Find(bone);
                if (boneObject == null) Debug.LogError($"{bone} object not found");
                vertexBones.Add(boneObject);
            }
            unmodifiedVertices = new Vector3[]
            {
                GameObject.Find("Nose.003_end").transform.localPosition,
                GameObject.Find("Nose.004_end").transform.localPosition,
                GameObject.Find("Nose.002_end").transform.localPosition
            };
            unmodifiedLen = Vector3.Distance(unmodifiedVertices[0], unmodifiedVertices[1]);
        }

        public void HandleFaceUpdate(float[] vertices)
        {
            Vector3 currentVertex = new Vector3();
            Vector3 rootVertex = new Vector3();
            Vector3 p1 = new Vector3(), p2 = new Vector3(), p3 = new Vector3();
            //rootVertex.GetVector(vertices, 0);
            rootVertex.x = vertices[0];
            rootVertex.y = -vertices[1];
            rootVertex.z = vertices[2];
            faceRoot.transform.position = rootVertex * faceScale;
            currentVertex.x = vertices[3];
            currentVertex.y = -vertices[4];
            currentVertex.z = vertices[5];
            p1 = currentVertex;
            currentVertex.x = vertices[6];
            currentVertex.y = -vertices[7];
            currentVertex.z = vertices[8];
            p2 = currentVertex;
            currentVertex.x = vertices[9];
            currentVertex.y = -vertices[10];
            currentVertex.z = vertices[11];
            p3 = currentVertex;
            float ratio = Mathf.Sqrt(unmodifiedLen / Vector3.Distance(p1, p2)) * faceScale;
            Debug.Log($"ratio = { ratio }, z = {(p1.z + p2.z)/2}");
            //Matrix4x4 transformationMatrix = GetTransformationMatrix(p1, p2, p3);
            //p1.GetVector(vertices, 3);
            //p2.GetVector(vertices, 6);
            //p3.GetVector(vertices, 9);
            //faceRoot.transform.rotation = Quaternion.Euler(RollPitchYaw(p1, p2, p3));
            int i = 0;
            foreach (var bone in vertexBones)
            {
                currentVertex.x = vertices[i++];
                currentVertex.y = -vertices[i++];
                currentVertex.z = vertices[i++];
                bone.transform.position = currentVertex * faceScale;// - rootVertex;// * ratio;
            }
        }
        public void SimpleFaceUpdate(float[] vertices, List<GameObject> vertexObjects)
        {
            Vector3 currentVertex = new Vector3();
            Vector3 rootVector = new Vector3();
            Vector3? p1 = null, p2 = null, p3 = null;
            float scale = 2;
            currentVertex.x = vertices[0];
            currentVertex.y = 1-vertices[1];
            currentVertex.z = vertices[2];
            vertexObjects[0].transform.position = currentVertex * scale;
            faceRoot1.transform.position = currentVertex * scale;
            rootVector = currentVertex;
            currentVertex.x = vertices[3];
            currentVertex.y = -vertices[4];
            currentVertex.z = vertices[5];
            rootVector.y = rootVector.y - 1;
            rootVector = currentVertex - rootVector;
            vertexObjects[1].transform.position = currentVertex * scale;
            //Quaternion rotation = Quaternion.FromToRotation(Vector3.one, rootVector);
            //faceRoot1.transform.position = currentVertex * faceScale;
            //faceRoot.transform.rotation = Quaternion.LookRotation(rootVector, Vector3.forward); 
            int i = 6, j = 2;
            foreach (var bone in vertexBones)
            {
                currentVertex.x = vertices[i++];
                currentVertex.y = -vertices[i++];
                currentVertex.z = vertices[i++];
                vertexObjects[j++].transform.position = currentVertex * scale;
                switch (bone.name)
                {
                    case "Forehead":
                        p1 = currentVertex;
                        break;
                    case "Cheek.R.002":
                        p2 = currentVertex;
                        break;
                    case "Cheek.L.002":
                        p3 = currentVertex;
                        break;
                }
            }
            faceRoot1.transform.rotation = Quaternion.Euler(RollPitchYaw(p3.Value, p2.Value, p1.Value));
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

        public void HandleFaceUpdate(FaceKeypoints keypoints)
        {
            //This will only handle face expressions from now
            //faceRoot.transform.position = keypoints.pos.ToVector() * faceScale;
            //faceRoot.transform.rotation = keypoints.rot.ToQuaternion();
            if (keypoints.vertices.Count > 0)
            {
                int i = 0;
                foreach (var bone in vertexBones)
                {
                    bone.transform.localPosition = keypoints.vertices[i++].ToVector();
                }
            }
        }

        public static Vector3 RollPitchYaw(Vector3 a, Vector3 b, Vector3 c)
        {
            var qb = b - a;
            var qc = (Vector3)(c - a);
            var n = Vector3.Cross(qb, qc);

            var unitZ = Unit(n);
            var unitX = Unit(qb);
            var unitY = Vector3.Cross(unitZ, unitX);

            var beta = MathF.Asin(unitZ.x);
            var alpha = MathF.Atan2(-unitZ.y, unitZ.z);
            var gamma = MathF.Atan2(-unitY.x, unitX.x);

            return new Vector3(-NormalizeAngle(alpha), 180-NormalizeAngle(beta), 180-NormalizeAngle(gamma));
        }

        public static Vector2 Unit(Vector2 vector) => vector / vector.magnitude;
        public static Vector3 Unit(Vector3 vector) => vector / vector.magnitude;

        public static float NormalizeAngle(float radians)
        {
            var twoPi = MathF.PI * 2;
            var angle = radians % twoPi;
            angle = angle > MathF.PI ? angle - twoPi : angle < -MathF.PI ? twoPi + angle : angle;
            return 180*angle / MathF.PI;
        }

        public static Matrix4x4 GetTransformationMatrix(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            // Construct the matrix of equations
            Matrix4x4 matrixOfEquations = new Matrix4x4(
                new Vector4(v0.x, v0.y, v0.z, 1),
                new Vector4(v1.x, v1.y, v1.z, 1),
                new Vector4(v2.x, v2.y, v2.z, 1),
                new Vector4(1, 1, 1, 1)
            ).inverse;

            // Solve the system of equations using the inverse of the matrix of equations
            Matrix4x4 solutions = matrixOfEquations * new Matrix4x4(
                new Vector4(unmodifiedVertices[0].x, unmodifiedVertices[0].y, unmodifiedVertices[0].z, 1),
                new Vector4(unmodifiedVertices[1].x, unmodifiedVertices[1].y, unmodifiedVertices[1].z, 1),
                new Vector4(unmodifiedVertices[2].x, unmodifiedVertices[2].y, unmodifiedVertices[2].z, 1),
                new Vector4(1, 1, 1, 1)
            );

            // Construct the transformation matrix from the solutions
            Matrix4x4 transformationMatrix = new Matrix4x4(
                new Vector4(solutions[0, 0], solutions[1, 0], solutions[2, 0], solutions[3, 0]),
                new Vector4(solutions[0, 1], solutions[1, 1], solutions[2, 1], solutions[3, 1]),
                new Vector4(solutions[0, 2], solutions[1, 2], solutions[2, 2], solutions[3, 2]),
                new Vector4(solutions[0, 3], solutions[1, 3], solutions[2, 3], solutions[3, 3])
            );

            return transformationMatrix;
        }

        // Apply the transformation matrix to a vertex in the modified space
        public Vector3 TransformVertex(Vector3 vertex, Matrix4x4 transformationMatrix)
        {
            Vector4 homogenousCoord = new Vector4(vertex.x, vertex.y, vertex.z, 1);
            Vector4 transformedCoord = transformationMatrix * homogenousCoord;
            return new Vector3(transformedCoord.x, transformedCoord.y, transformedCoord.z);
        }
    }
}
