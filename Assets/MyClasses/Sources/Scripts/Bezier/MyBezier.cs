/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyBezier (version 1.0)
 */

using UnityEngine;

namespace MyClasses
{
    public class MyBezier
    {
        #region ----- Public Method -----

        /// <summary>
        /// Returns a point on a linear bezier curve by time.
        /// </summary>
        /// <param name="t">0 -> 1</param>
        public static Vector3 GetLinearBezierPoint(Vector3 p0, Vector3 p1, float t)
        {
            t = Mathf.Clamp01(t);
            return p0 + (t * (p1 - p0));
        }

        /// <summary>
        /// Returns a point on a quadratic bezier curve by time.
        /// </summary>
        /// <param name="t">0 -> 1</param>
        public static Vector3 GetQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            float tt = t * t;
            float u = 1 - t;
            float uu = u * u;
            return (uu * p0) + (2 * u * t * p1) + (tt * p2);
        }

        /// <summary>
        /// Returns a point on a cubic bezier curve by time.
        /// </summary>
        /// <param name="t">0 -> 1</param>
        public static Vector3 GetCubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float tt = t * t;
            float ttt = tt * t;
            float u = 1 - t;
            float uu = u * u;
            float uuu = uu * u;
            return (uuu * p0) + (3 * uu * t * p1) + (3 * u * tt * p2) + (ttt * p3);
        }

        #endregion
    }
}
