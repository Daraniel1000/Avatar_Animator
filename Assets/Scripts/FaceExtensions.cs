using UnityEngine;

namespace Assets.Scripts
{
    public static class FaceExtensions
    {
        public static Vector3 divide(this Vector3 a, Vector3 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;
            return a;
        }

    }
}
