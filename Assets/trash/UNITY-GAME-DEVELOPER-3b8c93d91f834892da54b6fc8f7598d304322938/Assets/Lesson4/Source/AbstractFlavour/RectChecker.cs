using System;
using UnityEngine;

namespace Lesson4.AbstractFlavour
{
    /// <summary>
    /// Concrete implementation of DistanceCheckerBase, its inheritant, child
    /// </summary>
    [Serializable]
    public class RectChecker : DistanceCheckerBase
    {
        [SerializeField, Tooltip("Additional value for rect checker, width and height")] private Vector2 _extends;

        /// <summary>
        /// Expansion of a constructor for Rect implementation.
        /// Base constructor of DistanceCheckerBase is called by default
        /// before executing Rect one
        /// </summary>
        /// <param name="position"></param>
        /// <param name="extends"></param>
        public RectChecker(Vector3 position, Vector2 extends) : base(position)
        {
            _extends = extends;
        }

        /// <summary>
        /// Implementation of promised by DistanceCheckerBase method IsInRange
        /// Rect check consist of checking if x coordinate is between rect bounds in x dimension
        /// And the same for the z dimension. Logic operation AND is written in c# as '&&'
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override bool IsInRange(Vector3 position)
        {
            Vector2 halfExtends = _extends * 0.5f;
            return IsBetween(position.x, this.position.x - halfExtends.x, this.position.x + halfExtends.x)
                   && IsBetween(position.z, this.position.z - halfExtends.y, this.position.z + halfExtends.y);
        }
        
        /// <summary>
        /// This method checks if given value is in between og given min and max value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private bool IsBetween(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Expansion of Gizmos drawing, calls DistanceCheckerBase version
        /// using base.DrawGizmos after setting color but before drawing a rect
        /// </summary>
        public override void DrawGizmos()
        {
            Gizmos.color = Color.cyan;
            base.DrawGizmos();
            DrawRect();
        }

        /// <summary>
        /// Draw a rect using Gizmos.DrawLineStrip, which expects set of points to connect into strip
        /// and notion if to loop a strip
        /// Every point of a rect is a corner of a rect
        /// DistanceCheckerBase protected method DrawRays is called to draw rays up from every point on the rect
        /// </summary>
        private void DrawRect()
        {
            Vector2 halfExtends = _extends * 0.5f;
            
            Vector3[] corners = new Vector3[4];
            corners[0] = position + new Vector3(halfExtends.x, 0f, halfExtends.y);
            corners[1] = position + new Vector3(halfExtends.x, 0f, -halfExtends.y);
            corners[2] = position + new Vector3(-halfExtends.x, 0f, -halfExtends.y);
            corners[3] = position + new Vector3(-halfExtends.x, 0f, halfExtends.y);
            Gizmos.DrawLineStrip(corners, true);
            DrawRays(corners);
        }
    }
}