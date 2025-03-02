using UnityEngine;

namespace Lesson4.AbstractFlavour
{
    public abstract class DistanceCheckerBase
    {
        /// <summary>
        /// Constant value, radius of Gizmos sphere
        /// </summary>
        public const float GIZMOS_SPHERE_RADIUS = .125f;
        /// <summary>
        /// Constant value, length of Gizmos rays
        /// </summary>
        public const float GIZMOS_RAY_DISTANCE = 100f;
        
        //protected value, other classes and scripts cannot use it, but children can
        [SerializeField, Tooltip("Position of the center of the zone (world space)")] protected Vector3 position;
        //private value, even children can't use it
        [SerializeField, Tooltip("Demonstration of private value")] private Vector3 _hiddenPosition;

        //public property, other classes and scripts can get value but cannot set it
        public Vector3 Position
        {
            get
            {
                return position;
            }
        }
        //Shortened version of the property above
        //public Vector3 Position => position;

        /// <summary>
        /// Base constructor of instance of DistanceCheckerBase, position is set up as a default position
        /// </summary>
        /// <param name="position"></param>
        protected DistanceCheckerBase(Vector3 position)
        {
            this.position = position;
        }
        
        /// <summary>
        /// Abstract method, promise that abstraction will answer if point is in range of some zone
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public abstract bool IsInRange(Vector3 position);

        /// <summary>
        /// virtual method, by default will draw center point, but can be overriden to draw more
        /// </summary>
        public virtual void DrawGizmos()
        {
            Gizmos.DrawSphere(position, GIZMOS_SPHERE_RADIUS);
        }

        /// <summary>
        /// Sample of private method, even children can't use it
        /// </summary>
        private void SuperPrivateMethod()
        {
            // Do stuff
        }
        
        /// <summary>
        /// Protected method, draws rays up from points given to it, children can use it to complete drawing of the zone
        /// </summary>
        /// <param name="points"></param>
        protected void DrawRays(Vector3[] points)
        {
            for (int i = 0; i < points.Length; ++i)
            {
                Gizmos.DrawRay(points[i], Vector3.up * GIZMOS_RAY_DISTANCE);
            }
        }
    }
}