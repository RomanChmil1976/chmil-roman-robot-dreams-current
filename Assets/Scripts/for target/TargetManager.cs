using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance;

    private List<Target> activeTargets = new List<Target>();

    public static event Action<Target> onTargetSpawn;
    public static event Action<Target> onTargetDespawn;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Register(Target target)
    {
        if (!activeTargets.Contains(target))
        {
            activeTargets.Add(target);
            onTargetSpawn?.Invoke(target);
        }
    }

    public void Unregister(Target target)
    {
        if (activeTargets.Contains(target))
        {
            activeTargets.Remove(target);
            onTargetDespawn?.Invoke(target);
        }
    }
}