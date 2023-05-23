using Assets.Scenes.FaceTracking;
using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class BodyHelper
    {
        private readonly BonePos leftArmHint, rightArmHint, leftArmTarget, rightArmTarget;
        private readonly BoneRot spineRotator;
        private readonly List<GameObject> previewObjects = new(33);
        private readonly GameObject rArmRoot, lArmRoot;
        private float scaleY = -1f;
        private readonly BoneRot[] hands;

        public BodyHelper(GameObject prefab)
        {
            spineRotator = new BoneRot("Spine.001");

            leftArmTarget = new BonePos("LH IK Target");
            leftArmHint = new BonePos("LH IK Hint");
            rightArmTarget = new BonePos("RH IK Target");
            rightArmHint = new BonePos("RH IK Hint");

            rArmRoot = GameObject.Find("Arm.R");
            lArmRoot = GameObject.Find("Arm.L");

            hands = new BoneRot[] { new("LH IK Rotator"), new("RH IK Rotator") };

            for (int i = 0; i<=42; ++i)
            {
                previewObjects.Add(GameObject.Instantiate(prefab));
            }
        }

        public void Preview(BodyData data)
        {
            if (data.Hands.Landmarks.Count > 0)
            {
                for (int i = 0; i < 21; i++)
                {
                    previewObjects[i].transform.position = data.Hands.Landmarks[0][i].ToVector().scaleY(-1);
                    previewObjects[i].name = i.ToString();
                }
                for (int i = 0; i < 21; i++)
                {
                    previewObjects[i+21].transform.position = data.Hands.Landmarks2D[0][i].ToVector().scaleY(-1);
                    previewObjects[i+21].name = i.ToString();
                }
            }
            //for (int i = 0; i < data.Body.Count; i++)
            //{
            //    previewObjects[i].transform.position = data.Body[i].ToVector().scaleY(scaleY);
            //    previewObjects[i].name = i.ToString();
            //}
        }

        public void HandleBodyUpdate(BodyData data)
        {

            int i = 0;
            foreach (var hand in data.Hands.MultiHandedness)
            {
                if (hand.score < .7f)
                    continue;
                int j = hand.label == "Left" ? 1 : 0;
                HandleHandUpdate(data.Hands.Landmarks[i], data.Hands.Landmarks2D[i++], j);
            }

            // Get body twist and angle from shoulder vector
            Vector3 shoulderRot = GetShoulderRot(data);
            spineRotator.SetRotation(Quaternion.Euler(shoulderRot * 180));

            // Arm location rig
            Vector3 rArmOffset = rArmRoot.transform.position - data.Body[12].ToVector().scaleY(scaleY);
            Vector3 lArmOffset = lArmRoot.transform.position - data.Body[11].ToVector().scaleY(scaleY);
            rightArmTarget.SetPosition(data.Body[16].ToVector().scaleY(scaleY) + rArmOffset);
            rightArmHint.SetPosition(data.Body[14].ToVector().scaleY(scaleY) + rArmOffset);
            leftArmTarget.SetPosition(data.Body[15].ToVector().scaleY(scaleY) + lArmOffset);
            leftArmHint.SetPosition(data.Body[13].ToVector().scaleY(scaleY) + lArmOffset);

            // Neck location rig, currently retired
            //Vector3 shoulderMidpoint = Vector3.Lerp(data.Body[12].ToVector(), data.Body[11].ToVector(), 0.5f).scaleY(scaleY);
            //Vector3 headMidpoint = Vector3.Lerp(data.Body[8].ToVector(), data.Body[7].ToVector(), 0.5f).scaleY(-1.1f);
            //neckTarget.SetRelativePosition(headMidpoint, shoulderBone.transform.position - shoulderMidpoint);
            //neckTarget.SetRotation(faceBone.bone.transform.rotation);
            //neckTarget.SetRotation(Quaternion.Euler(faceRot * 180));
        }

        private Vector3 GetShoulderRot(BodyData data)
        {
            // Get body twist and angle from shoulder vector
            Vector3 spine = HelperExtensions.RollPitchYaw(data.Body[11].ToVector().scaleY(-1), data.Body[12].ToVector().scaleY(-1));
            if (spine.y > .5f)
                spine.y -= 2;
            spine.y += .5f;
            // Prevent jumping between left and right shoulder tilt
            if (spine.z > 0)
                spine.z = 1 + spine.z;
            if (spine.z < 0)
                spine.z = -1 + spine.z;
            // Cannot get x from these 2
            spine.x = 0;

            return spine;
        }

        private void HandleHandUpdate(List<Vec3> data, List<Vec3>data2D, int idx)
        {
            var rot = GetHandLookVectors(data, idx);
            hands[idx].SetRotation(Quaternion.LookRotation(rot.Item1, rot.Item2));
        }

        private Tuple<Vector3, Vector3> GetHandLookVectors(List<Vec3> data, int idx)
        {
            var points = new Vector3[] { data[0].ToVector().scaleY(scaleY), data[5].ToVector().scaleY(scaleY), data[17].ToVector().scaleY(scaleY) };
            var front = (idx == 0 ? 1 : -1) * Vector3.Cross(points[2] - points[0], points[1] - points[2]).normalized;
            var midpoint = Vector3.Lerp(points[1], points[2], .5f);
            var up = midpoint - points[0];
            up.Normalize();
            return new Tuple<Vector3, Vector3>(front, up);
        }

        //public Vector3 GetFaceRot(FaceMediapipeData data)
        //{
        //    // Find 3 vectors that form a plane to represent the head
        //    Vector3[] plane = FaceEulerPlane(data);
        //    // Get RollPitchYaw from plane
        //    Vector3 faceRot = HelperExtensions.RollPitchYaw(plane[0], plane[1], plane[2]);
        //    // Adjust and set face rotation
        //    faceRot.z = -faceRot.z;
        //    faceRot.x = -faceRot.x;

        //    return faceRot;
        //}

        //public static Vector3[] FaceEulerPlane(FaceMediapipeData data)
        //{
        //    Vector3 bottomMidpoint = Vector3.Lerp(data.BottomRight.ToVector(), data.BottomLeft.ToVector(), 0.5f);

        //    return new Vector3[] { data.TopLeft.ToVector(), data.TopRight.ToVector(), bottomMidpoint };
        //}
    }
}
