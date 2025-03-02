using UnityEngine;

namespace Lesson4.InterfaceFlavour
{
    /// <summary>
    /// Interface that promises that some position of center of the zone and method to check
    /// if some point is inside said abstract zone will be implemented
    /// </summary>
    public interface IDistanceChecker
    {
        Vector3 Position { get; }
        bool IsInRange(Vector3 position);
    }
}