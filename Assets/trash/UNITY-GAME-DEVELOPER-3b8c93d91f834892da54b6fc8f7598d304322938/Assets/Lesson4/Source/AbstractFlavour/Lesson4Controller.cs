using UnityEngine;

namespace Lesson4.AbstractFlavour
{
    public class Lesson4Controller : MonoBehaviour
    {
        [SerializeField, Tooltip("Reference to transform of an object we check distance with")] private Transform _target;
        [SerializeReference, Tooltip("Serialized reference to instance of checker class")] private DistanceCheckerBase _distanceChecker;

        /// <summary>
        /// Context menu option to inject concrete checker (Circle)
        /// </summary>
        [ContextMenu("Set Circle Checker")]
        private void SetCircleChecker()
        {
            Vector3 position;
            if (_distanceChecker != null)
            {
                position = _distanceChecker.Position;
            }
            else
            {
                position = Vector3.zero;
            }
            
            // This is a shortcut version of the code above
            //position = _distanceChecker?.Position ?? Vector3.zero;
            _distanceChecker = new CircleChecker(position, 3f);
        }
        
        /// <summary>
        /// Context menu option to inject concrete checker (Rect)
        /// </summary>
        [ContextMenu("Set Rect Checker")]
        private void SetRectChecker()
        {
            Vector3 position;
            if (_distanceChecker != null)
            {
                position = _distanceChecker.Position;
            }
            else
            {
                position = Vector3.zero;
            }
            
            // This is a shortcut version of the code above
            //position = _distanceChecker?.Position ?? Vector3.zero;
            _distanceChecker = new RectChecker(position, Vector2.one);
        }

        [ContextMenu("Clear Checker")]
        private void ClearChecker()
        {
            _distanceChecker = null;
        }
        
        /// <summary>
        /// Ask DistanceCheckerBase instance if point is in range without knowing concrete implementation
        /// </summary>
        [ContextMenu("Check")]
        private void Check()
        {
            if (_distanceChecker != null)
            {
                Debug.Log($"InRange (Abstract Flavour): {_distanceChecker.IsInRange(_target.position)}");
            }
            //Shortened version
            //Debug.Log($"InRange (Interface Flavour): {_distanceChecker?.IsInRange(_target.position).ToString() ?? "NULL"}");
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_distanceChecker != null)
            {
                // Command DistanceCheckerBase instance to draw bounds with Gizmos without knowing concrete implementation
                _distanceChecker.DrawGizmos();
            }
            //_distanceChecker?.DrawGizmos();
        }
    }
}