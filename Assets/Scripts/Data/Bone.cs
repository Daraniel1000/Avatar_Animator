using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class Bone: BoneBase
    {
        public readonly OneEuroFilter<Vector3> posFilter = new(20, 0.5f, 0.2f);
        public readonly OneEuroFilter<Quaternion> rotFilter = new(20, 0.5f, 0.1f);

        private readonly Quaternion baseRotation, baseLocalRot;

        public Bone(string name): base(name)
        {
            if (bone != null)
            {
                baseRotation = bone.transform.rotation;
                baseLocalRot = bone.transform.localRotation;
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

        public void SetRelativeRotation(Quaternion rotation)
        {
            bone.transform.rotation = rotFilter.Filter(rotation, Time.unscaledTime) * baseRotation;
        }
    }

    public class BonePos : BoneBase
    {
        private readonly OneEuroFilter<Vector3> posFilter;

        public BonePos(string name) : base(name) 
        {
            posFilter = new(20, 0.5f, 0.1f);
        }

        public BonePos(string name, float mincutoff, float beta) : base(name)
        {
            posFilter = new(20, mincutoff, beta);
        }

        public void SetPosition(Vector3 position)
        {
            bone.transform.position = posFilter.Filter(position, Time.unscaledTime);
        }

        public Vector3 SetRelativePosition(Vector3 position, Vector3 offset)
        {
            return bone.transform.position = posFilter.Filter(position, Time.unscaledTime) + offset;
        }
    }

    public class BoneRot : BoneBase
    {
        private readonly Quaternion baseRotation;
        private readonly OneEuroFilter<Quaternion> rotFilter = new(20, 0.6f, 0.5f);
        private readonly Vector3 baseLocalRot;
        private static readonly float radconst = Mathf.PI/180f;

        public BoneRot(string name) : base(name)
        {
            if (bone != null)
            {
                baseRotation = bone.transform.rotation;
                baseLocalRot = bone.transform.localEulerAngles;
            }
        }

        public void SetRotation(Quaternion rotation)
        {
            bone.transform.rotation = rotFilter.Filter(rotation, Time.unscaledTime) * baseRotation;
        }

        public void SetRelativeRotation(Quaternion rotation)
        {
            bone.transform.localRotation = rotFilter.Filter(rotation, Time.unscaledTime);// * baseRotation;
        }

        public void SetXRotation(float x)
        {
            if (x < 45) x = Mathf.Sin(2 * x * radconst);
            else x *= 1 + Mathf.Sin(3.6f * (x - 45) * radconst) / 2;
            bone.transform.localRotation = rotFilter.Filter(Quaternion.Euler(x, baseLocalRot.y, baseLocalRot.z), Time.unscaledTime);
        }

        public void SetXRotationSeg2(float x)
        {
            if (x <= 45) x = Mathf.Sin(2*x*radconst);
            else x *= 1 + Mathf.Sin(3.6f*(x-45)*radconst)/2;
            bone.transform.localRotation = rotFilter.Filter(Quaternion.Euler(x, baseLocalRot.y, baseLocalRot.z), Time.unscaledTime);
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

    public abstract class BoneBase
    {
        public GameObject bone { get; private set; }

        public BoneBase(string name)
        {
            bone = GameObject.Find(name);
            if (bone == null)
            {
                Debug.Log($"{name} not found");
            }
        }
    }
}
