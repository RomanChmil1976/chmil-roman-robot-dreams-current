using System;
using UnityEngine;

namespace Lesson4.InterfaceFlavour
{
    /// <summary>
    /// Completely identical to RectChecker from AbstractFlavour namespace
    /// </summary>
    [Serializable]
    public class RectChecker : DistanceCheckerBase
    {
        [SerializeField] private Vector2 _extends;

        public RectChecker(Vector3 position, Vector2 extends) : base(position)
        {
            _extends = extends;
        }

        public override bool IsInRange(Vector3 position)
        {
            Vector2 halfExtends = _extends * 0.5f;
            return IsBetween(position.x, this.position.x - halfExtends.x, this.position.x + halfExtends.x)
                   && IsBetween(position.z, this.position.z - halfExtends.y, this.position.z + halfExtends.y);
        }
        
        private bool IsBetween(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.cyan;
            base.DrawGizmos();
            DrawRect();
        }

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