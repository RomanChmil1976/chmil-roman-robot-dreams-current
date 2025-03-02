using System;
using UnityEngine;

namespace Lesson4.AbstractFlavour
{
    /// <summary>
    /// Concrete implementation of DistanceCheckerBase, its inheritant, child
    /// </summary>
    [Serializable]
    public class CircleChecker : DistanceCheckerBase
    {
        [SerializeField, Tooltip("Additional value for circle checker, radius of the circle")] private float _radius;

        /// <summary>
        /// Expansion of a constructor for Circle implementation.
        /// Base constructor of DistanceCheckerBase is called by default
        /// before executing Circle one
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        public CircleChecker(Vector3 position, float radius) : base(position)
        {
            _radius = radius;
        }

        /// <summary>
        /// Implementation of promised by DistanceCheckerBase method IsInRange
        /// uses formula of a circle to determine if point is in circle
        /// given O as circle center, A as point, R as radius
        /// (O.x - A.x)^2 + (O.y - A.y)^2 less or equals R^2
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override bool IsInRange(Vector3 position)
        {
            float xFactor = position.x - this.position.x;
            float zFactor = position.z - this.position.z;
            return xFactor * xFactor + zFactor * zFactor <= _radius * _radius;
        }

        /// <summary>
        /// Expansion of Gizmos drawing, calls DistanceCheckerBase version
        /// using base.DrawGizmos after setting color but before drawing circle
        /// </summary>
        public override void DrawGizmos()
        {
            Gizmos.color = Color.green;
            base.DrawGizmos();
            DrawCircle();
        }

        /// <summary>
        /// Draw a circle using Gizmos.DrawLineStrip, which expects set of points to connect into strip
        /// and notion if to loop a strip
        /// Every point of a circle is a point where x is cosine of angle and y is sine of angle
        /// Angle is changed by step, which is 360 degrees divided by number of points
        /// DistanceCheckerBase protected method DrawRays is called to draw rays up from every point on the circle
        /// </summary>
        private void DrawCircle()
        {
            float angleStep = 2f * Mathf.PI / 24f;
            Vector3[] circlePositions = new Vector3[24];
            circlePositions[0] = position + new Vector3(_radius, 0f, 0f);
            for (int i = 1; i < 24; ++i)
            {
                float angle = angleStep * i;
                circlePositions[i] = position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _radius;
            }
            Gizmos.DrawLineStrip(circlePositions, true);
            DrawRays(circlePositions);
        }
    }
}