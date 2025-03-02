using UnityEngine;

namespace Lesson5
{
    public class PlayerLoopSample : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("Awake");
        }

        private void Start()
        {
            Debug.Log("Start");
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable");
        }

        private void Update()
        {
            Debug.Log("Update");
        }

        private void LateUpdate()
        {
            Debug.Log("LateUpdate");
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable");
        }

        private void OnDestroy()
        {
            Debug.Log("OnDestroy");
        }
    }
}