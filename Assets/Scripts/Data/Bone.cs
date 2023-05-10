using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class Bone
    {
        public GameObject bone { get; private set; }

        public readonly OneEuroFilter<Vector3> posFilter = new(20, 0.5f, 0.2f);
        public readonly OneEuroFilter<Quaternion> rotFilter = new(20, 0.5f, 0.1f);

        public Bone(string name)
        {
            bone = GameObject.Find(name);
            if (bone == null)
            {
                Debug.Log($"{name} not found");
            }
        }

        public void SetPosition(Vector3 position)
        {
            bone.transform.position = posFilter.Filter(position, Time.unscaledTime);
        }

        public void SetRelativePosition(Vector3 position, Vector3 offset)
        {
            bone.transform.position = posFilter.Filter(position, Time.unscaledTime) + offset;
        }

        public void SetRotation(Quaternion rotation)
        {
            bone.transform.rotation = rotFilter.Filter(rotation, Time.unscaledTime);
        }
    }

    public class BonePos
    {
        public GameObject bone { get; private set; }
        
        private readonly OneEuroFilter<Vector3> boneFilter = new(20, 0.5f, 0.1f);

        public BonePos(string name)
        {
            bone = GameObject.Find(name);
            if (bone == null)
            {
                Debug.Log($"{name} not found");
            }
        }

        public void SetPosition(Vector3 position)
        {
            bone.transform.position = boneFilter.Filter(position, Time.unscaledTime);
        }
    }

    public class BoneRot
    {
        public GameObject bone { get; private set; }

        private readonly Quaternion baseRotation;
        private readonly OneEuroFilter<Quaternion> boneFilter = new(20, 0.6f, 0.5f);

        public BoneRot(string name)
        {
            bone = GameObject.Find(name);
            if (bone == null)
            {
                Debug.Log($"{name} not found");
            }
            else
            {
                baseRotation = bone.transform.rotation;
            }
        }

        public void SetRotation(Quaternion rotation)
        {
            bone.transform.rotation = boneFilter.Filter(rotation, Time.unscaledTime) * baseRotation;
        }
    }

    public class MultiBoneRot
    {
        public List<GameObject> bones { get; private set; } = new List<GameObject>();

        private readonly List<Quaternion> baseRotations = new List<Quaternion>();
        private readonly OneEuroFilter<Quaternion> boneFilter = new(20, 0.6f, 0.5f);

        public MultiBoneRot(params string[] names)
        {
            GameObject refObj;
            foreach (var name in names)
            {
                refObj = GameObject.Find(name);
                if (refObj == null)
                {
                    Debug.Log($"{name} not found");
                }
                else
                {
                    bones.Add(refObj);
                    baseRotations.Add(refObj.transform.localRotation);
                }
            }
        }

        public void SetRotation(Vector3 rotation)
        {
            Quaternion filtered = boneFilter.Filter(Quaternion.Euler(rotation), Time.unscaledTime);
            for (int i = 0; i < bones.Count; i++)
            {
                bones[i].transform.localRotation = filtered * baseRotations[i];

            }
        }
    }
}
