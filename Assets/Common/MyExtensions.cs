using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedWork
{
    public static class MyExtensions
    {
        //public static bool IsMud(this Component component) => component.CompareTag(Tag.Mud);

        //public static Vector3 Shrink(this Vector3 from, Vector2 to, float byX, float byY)
        //{
        //    return new Vector3(Mathf.MoveTowards(from.x, to.x, byX), Mathf.MoveTowards(from.y, to.y, byY));
        //}

        /// <summary>
        /// calculates based on parameters, the amount of shrinking calls required to reach the min. 
        /// You need to make sure the methods gets called enough times to reach it tho.
        /// </summary>
        /// <param name="current">the current size</param>
        /// <param name="min">the min size to reach (if this is for example an enemy, which dies, minimum might not be seen)</param>
        /// <param name="shrinkCount">the amount of times this method call is required to reach the min size</param>
        /// <returns></returns>
        public static Vector3 Shrink(this Vector3 current, float minX, float minY, float shrinkCount) =>
            current.Shrink(new Vector3(minX, minY), shrinkCount);

        public static Vector3 Shrink(this Vector3 current, Vector3 min, float shrinkCount)
        {
            return new Vector3(
                Mathf.MoveTowards(current.x, min.x, (current.x - min.x) / shrinkCount),
                Mathf.MoveTowards(current.y, min.y, (current.y - min.y) / shrinkCount));
        }

        public static Quaternion FlipRotation2D(this Transform transform)
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.y = rotation.y == 0.0f
                ? 180.0f
                : 0.0f;
            return Quaternion.Euler(rotation);
        }
    }
}
