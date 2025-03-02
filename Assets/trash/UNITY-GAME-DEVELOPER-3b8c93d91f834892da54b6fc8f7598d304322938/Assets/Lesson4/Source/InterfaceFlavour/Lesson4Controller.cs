using UnityEngine;

namespace Lesson4.InterfaceFlavour
{
    /// <summary>
    /// Basically the same as in AbstractFlavour, but we store instances of IDistanceChecker interface and
    /// IDistanceDrawable interface in order to give commands to check for distance and draw Gizmos
    /// </summary>
    public class Lesson4Controller : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeReference, Tooltip("Serialized reference of instance that implements IDistanceChecker interface")] private IDistanceChecker _distanceChecker;

        /// <summary>
        /// Serialized reference of instance that implements IDistanceDrawable interface
        /// </summary>
        private IDistanceDrawable _distanceDrawable;

        /// <summary>
        /// Context menu option to inject concrete checker (Circle) in _distanceChecker (IDistanceChecker) and
        /// _distanceDrawable (IDistanceDrawable) cause CircleChecker inherits DistanceCheckerBase, which implements
        /// both interfaces
        /// </summary>
        [ContextMenu("Set Circle Checker")]
        private void SetCircleChecker()
        {
            Vector3 position = _distanceChecker?.Position ?? Vector3.zero;
            CircleChecker circleChecker = new CircleChecker(position, 3f);
            _distanceChecker = circleChecker;
            _distanceDrawable = circleChecker;
        }
        
        /// <summary>
        /// Context menu option to inject concrete checker (Rect) in _distanceChecker (IDistanceChecker) and
        /// _distanceDrawable (IDistanceDrawable) cause RectChecker inherits DistanceCheckerBase, which implements
        /// both interfaces
        /// </summary>
        [ContextMenu("Set Rect Checker")]
        private void SetRectChecker()
        {
            Vector3 position = _distanceChecker?.Position ?? Vector3.zero;
            RectChecker rectChecker = new RectChecker(position, Vector2.one);
            _distanceChecker = rectChecker;
            _distanceDrawable = rectChecker;
        }

        /// <summary>
        /// Ask IDistanceChecker instance if point is in range without knowing concrete implementation
        /// </summary>
        [ContextMenu("Check")]
        private void Check()
        {
            if (_distanceChecker != null)
            {
                Debug.Log($"InRange (Interface Flavour): {_distanceChecker.IsInRange(_target.position)}");
            }
            //Shortened version
            //Debug.Log($"InRange (Interface Flavour): {_distanceChecker?.IsInRange(_target.position).ToString() ?? "NULL"}");
        }

        private void OnDrawGizmosSelected()
        {
            if (_distanceDrawable != null)
            {
                // Command IDistanceDrawable instance to draw bounds with Gizmos without knowing concrete implementation
                _distanceDrawable.DrawGizmos();
            }
            //Shortened version
            //_distanceDrawable?.DrawGizmos();
        }
    }
}