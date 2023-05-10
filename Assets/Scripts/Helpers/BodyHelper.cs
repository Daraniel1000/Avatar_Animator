using Assets.Scripts.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class BodyHelper
    {
        private readonly BonePos leftArmTarget, leftArmHint, rightArmTarget, rightArmHint;
        private readonly Bone neckTarget;
        private readonly BoneRot faceBone, spine1;
        private readonly List<GameObject> previewObjects = new(33);
        private readonly GameObject rArmRoot, lArmRoot, shoulderBone;
        private float scaleY = -1f;

        public BodyHelper(GameObject prefab)
        {
            faceBone = new BoneRot("Root");
            spine1 = new BoneRot("Spine.001");

            leftArmTarget = new BonePos("LH IK Target");
            leftArmHint = new BonePos("LH IK Hint");
            rightArmTarget = new BonePos("RH IK Target");
            rightArmHint = new BonePos("RH IK Hint");
            neckTarget = new Bone("Neck IK Target");

            rArmRoot = GameObject.Find("Arm.R");
            lArmRoot = GameObject.Find("Arm.L");
            shoulderBone = GameObject.Find("Body");

            for (int i = 0; i<=34; ++i)
            {
                previewObjects.Add(GameObject.Instantiate(prefab));
            }
        }

        public void Preview(BodyData data)
        {
            if (data.Hands.Landmarks.Count > 0)
            {
                for (int i = 0; i < data.Hands.Landmarks[0].Count; i++)
                {
                    previewObjects[i].transform.position = data.Hands.Landmarks[0][i].ToVector().scaleY(-1);
                    previewObjects[i].name = i.ToString();
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
            //Vector3 faceRot = GetFaceRot(data.Face);
            //faceBone.SetRotation(Quaternion.Euler(faceRot * 180));
            
            // Get body twist and angle from shoulder vector
            Vector3 shoulderRot = GetShoulderRot(data);
            spine1.SetRotation(Quaternion.Euler(shoulderRot * 180));

            // Arm location rig
            Vector3 rArmOffset = rArmRoot.transform.position - data.Body[12].ToVector().scaleY(scaleY);
            Vector3 lArmOffset = lArmRoot.transform.position - data.Body[11].ToVector().scaleY(scaleY);
            Vector3 scale = new() { x = 1.1f, y = 1.1f, z = 1f };
            rightArmTarget.SetPosition(data.Body[16].ToVector().scaleY(scaleY) + rArmOffset);
            rightArmHint.SetPosition(data.Body[14].ToVector().scaleY(scaleY) + rArmOffset);
            leftArmTarget.SetPosition(data.Body[15].ToVector().scaleY(scaleY) + lArmOffset);
            leftArmHint.SetPosition(data.Body[13].ToVector().scaleY(scaleY) + lArmOffset);

            // Neck location rig
            Vector3 shoulderMidpoint = Vector3.Lerp(data.Body[12].ToVector(), data.Body[11].ToVector(), 0.5f).scaleY(scaleY);
            Vector3 headMidpoint = Vector3.Lerp(data.Body[8].ToVector(), data.Body[7].ToVector(), 0.5f).scaleY(-1.1f);
            neckTarget.SetRelativePosition(headMidpoint, shoulderBone.transform.position - shoulderMidpoint);
            //neckTarget.SetRotation(faceBone.bone.transform.rotation);
            //neckTarget.SetRotation(Quaternion.Euler(faceRot * 180));
        }

        public Vector3 GetShoulderRot(BodyData data)
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
