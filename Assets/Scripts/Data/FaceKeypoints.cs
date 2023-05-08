using MessagePack;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scenes.FaceTracking
{
    [MessagePackObject(true)]
    [Serializable]
    public class Vec3
    {
        public float x, y, z;

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vec3(Vector3 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
        }
        public static Vec3 FromVector(Vector3 other)
        {
            return new Vec3(other);
        }
        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }
    }

    [MessagePackObject(true)]
    [Serializable]
    public class Quat
    {
        public float x, y, z, w;

        public Quat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public Quat(Quaternion other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
            w = other.w;
        }
        public static Quat FromQuaternion(Quaternion other)
        {
            return new Quat(other);
        }
        public Quaternion ToQuaternion()
        {
            return new Quaternion(x, y, z, w);
        }
    }

    [MessagePackObject(true)]
    [Serializable]
    public class FaceKeypoints
    {
        public List<Vec3> vertices = new List<Vec3>();
        public Vec3 pos;
        public Quat rot;
    }
}
