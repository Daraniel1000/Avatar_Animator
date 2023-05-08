using Assets.Scripts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class BodyHelper
    {
        private readonly Bone leftArmTarget, leftArmHint, rightArmTarget, rightArmHint, neckTarget;
        private readonly GameObject faceRoot, bodyRoot, neck;
        private readonly List<GameObject> previewObjects = new(33);
        private readonly float scaleY = -4 / 3, scaleZPose = .5f;
        private readonly Vector3 rootBasePos;
        private readonly Quaternion neckBaseRot, bodyBaseRot;
        private readonly OneEuroFilter<Vector3> bodyFilter = new(20, 0.5f, 0.01f);
        private readonly OneEuroFilter<Vector3> faceRotFilter = new(20, .5f, .1f);

        public BodyHelper(GameObject prefab)
        {
            faceRoot = GameObject.Find("Root");
            bodyRoot = GameObject.Find("Body");
            rootBasePos = bodyRoot.transform.position;
            neck = GameObject.Find("Neck");
            leftArmTarget = new Bone("LH IK Target");
            leftArmHint = new Bone("LH IK Hint");
            rightArmTarget = new Bone("RH IK Target");
            rightArmHint = new Bone("RH IK Hint");
            neckTarget = new Bone("Neck IK Target");
            neckBaseRot = neck.transform.rotation;
            bodyBaseRot = bodyRoot.transform.rotation;
            for (int i = 0; i<=34; ++i)
            {
                previewObjects.Add(GameObject.Instantiate(prefab));
            }
        }

        public void HandleBodyUpdate(BodyData data)
        {
            // Find 3 vectors that form a plane to represent the head
            Vector3[] plane = FaceEulerPlane(data.Face);
            Vector3 rotate = faceRotFilter.Filter(HelperExtensions.RollPitchYaw(plane[0], plane[1], plane[2]), Time.unscaledTime);
            rotate.z = -rotate.z;
            rotate.x = -rotate.x;
            //We're basing body rotation on the head rotation - much less jitter
            faceRoot.transform.rotation = Quaternion.Euler(rotate * 180);
            //neck.transform.rotation = Quaternion.Euler(rotate * 90) * neckBaseRot;
            
            Vector3 shoulderMidpoint = bodyFilter.Filter(Vector3.Lerp(data.Body[12].ToVector(), data.Body[11].ToVector(), 0.5f).scaleY(scaleY), Time.unscaledTime);
            Vector3 spine = HelperExtensions.RollPitchYaw(data.Body[11].ToVector(), data.Body[12].ToVector());
            if (spine.y > .5f)
                spine.y -= 2;

            spine.y += .5f;

            // Prevent jumping between left and right shoulder tilt
            if (spine.z > 0)
                spine.z = 1 - spine.z;

            if (spine.z < 0)
                spine.z = -1 - spine.z;

            // Fix weird large numbers when 2 shoulder points get too close
            float turnAroundAmount = Math.Abs(spine.y).Remap(.2f, .4f);
            spine.z *= 1 - turnAroundAmount;
            spine.x = 0;
            bodyRoot.transform.SetPositionAndRotation(shoulderMidpoint, Quaternion.Euler(spine * 180) * bodyBaseRot);
            Vector3 rigTransform = rootBasePos - shoulderMidpoint;
            Vector3 headMidpoint = Vector3.Lerp(data.Body[8].ToVector(), data.Body[7].ToVector(), 0.5f).scaleY(scaleY);

            for (int i=0; i<data.Body.Count; ++i)
            {
                previewObjects[i].transform.position = data.Body[i].ToVector().scaleY(scaleY);
                previewObjects[i].name = i.ToString();
            }
            previewObjects[33].transform.position = data.Face.TopLeft.ToVector().scaleY(scaleY);
            previewObjects[34].transform.position = data.Face.TopRight.ToVector().scaleY(scaleY);

            rightArmTarget.SetPosition(data.Body[16].ToVector().scaleY(scaleY) + rigTransform);
            rightArmHint.SetPosition(data.Body[14].ToVector().scaleY(scaleY) + rigTransform);
            leftArmTarget.SetPosition(data.Body[15].ToVector().scaleY(scaleY) + rigTransform);
            leftArmHint.SetPosition(data.Body[13].ToVector().scaleY(scaleY) + rigTransform);
            neckTarget.SetPosition(headMidpoint + rigTransform);

        }

        private void RigArm(Vector3 armUpper, Vector3 armLower, bool side)
        {
            float invert = side ? 1f : -1f;

            armUpper.z *= -2.3f * invert;

            armUpper.y *= MathF.PI * invert;
            armUpper.y -= Math.Max(armLower.x, 0);
            armUpper.y -= -invert * Math.Max(armLower.z, 0);
            armUpper.x -= 0.3f * invert;

            armLower.z *= -2.14f * invert;
            armLower.y *= 2.14f * invert;
            armLower.x *= 2.14f * invert;

            // Clamp values to realistic humanoid limits
            armUpper.x = Math.Clamp(armUpper.x, -0.5f, MathF.PI);
            armLower.x = Math.Clamp(armLower.x, -0.3f, 0.3f);

            //arm.Hand.y = Math.Clamp(arm.Hand.z * 2, -0.6f, 0.6f); // sides
            //arm.Hand.z = arm.Hand.z * -2.3f * invert; // up and down
        }

        public static Vector3[] FaceEulerPlane(FaceMediapipeData data)
        {
            Vector3 bottomMidpoint = Vector3.Lerp(data.BottomRight.ToVector(), data.BottomLeft.ToVector(), 0.5f);

            return new Vector3[] { data.TopLeft.ToVector(), data.TopRight.ToVector(), bottomMidpoint };
        }
    }
}
