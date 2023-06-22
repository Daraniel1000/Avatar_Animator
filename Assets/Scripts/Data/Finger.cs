using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Data
{
    public class Finger
    {
        private GameObject bone;
        public readonly OneEuroFilter<Vector3> filter = new(20, 0.5f, 0.2f);

        public Vector3 offset;
        private float maxZ, minZ;
        private float delta = 10f;

        public Transform transform { get => bone.transform; }

        public Finger(string name)
        {
            bone = GameObject.Find(name);
            if (bone == null)
            {
                Debug.Log($"{name} not found");
            }
            else
            {
                offset = bone.transform.localEulerAngles;
                maxZ = offset.z + delta;
                minZ = offset.z - delta;
            }
        }

        public void LookAt(Vector3 target)
        {
            var rotation = Quaternion.LookRotation(filter.Filter(target, Time.unscaledTime)).eulerAngles;
            transform.eulerAngles = new Vector3(rotation.x + 90, rotation.y, rotation.z);
            //transform.LookAt(fingers[4].bone.transform, transform.forward);
            //transform.LookAt(target);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, offset.y, Mathf.Clamp(transform.localEulerAngles.z, minZ, maxZ));
        }

        public void constrainAngles()
        {
            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, offset.y, Mathf.Clamp(transform.localEulerAngles.z, minZ, maxZ));
        }
    }
}
