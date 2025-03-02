using UnityEngine;

namespace Lesson4.InterfaceFlavour
{
    /// <summary>
    /// Abstract class that implements IDistanceChecker and IDistanceDrawable interfaces, thus passing
    /// obligation to implement property Position, methods IsInRange and DrawGizmos, on its inheritants
    /// However, it implements Position and implements default DrawGizmos as a virtual method in order
    /// to save time and space, not rewrite same stuff in every inheritant
    /// </summary>
    public abstract class DistanceCheckerBase : IDistanceChecker, IDistanceDrawable
    {
        public const float GIZMOS_SPHERE_RADIUS = .125f;
        public const float GIZMOS_RAY_DISTANCE = 100f;
        
        [SerializeField] protected Vector3 position;

        public Vector3 Position
        {
            get
            {
                return position;
            }
        }
        //Shortened version of the property above
        //public Vector3 Position => position;

        protected DistanceCheckerBase(Vector3 position)
        {
            this.position = position;
        }
        
        public abstract bool IsInRange(Vector3 position);

        public virtual void DrawGizmos()
        {
            Gizmos.DrawSphere(position, GIZMOS_SPHERE_RADIUS);
        }

        protected void DrawRays(Vector3[] points)
        {
            for (int i = 0; i < points.Length; ++i)
            {
                Gizmos.DrawRay(points[i], Vector3.up * GIZMOS_RAY_DISTANCE);
            }
        }
    }
}