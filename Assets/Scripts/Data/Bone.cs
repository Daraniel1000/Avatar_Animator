using UnityEngine;

namespace Assets.Scripts.Data
{
    public class Bone
    {
        public GameObject bone { get; private set; }
        
        private readonly OneEuroFilter<Vector3> boneFilter = new(20, 0.5f, 0.1f);

        public Bone(string name)
        {
            bone = GameObject.Find(name);
            if (bone == null)
            {
                Debug.Log($"{name} not found");
            }
        }

        //public void SetRotation(Quaternion rotation)
        //{
        //    bone.transform.rotation = boneFilter.Filter(rotation, Time.unscaledTime) * baseRotation;
        //}

        public void SetPosition(Vector3 position)
        {
            bone.transform.position = boneFilter.Filter(position, Time.unscaledTime);
        }
    }
}
