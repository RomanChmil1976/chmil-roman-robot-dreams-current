using UnityEngine;
#if !UNITY_EDITOR && DEVELOPMENT_BUILD
using System;
using System.IO;
using UnityEngine.Profiling;
#endif

public class ProfilerManager : MonoBehaviour
{
    public static ProfilerManager Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void BeginProfiling()
    {
#if !UNITY_EDITOR && DEVELOPMENT_BUILD && PROFILER_ENABLED
        //Add config to profile or not application start
        string directory = Path.Combine(Application.persistentDataPath, "profiler-captures");
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
        DateTime now = DateTime.Now;
        Profiler.logFile = Path.Combine(directory, $"profiler-{now.Hour}-{now.Minute}-{now.Second}_{now.Day}-{now.Month}-{now.Year}.data");
        Profiler.enableBinaryLog = true;
        Profiler.enabled = true;
        Profiler.SetAreaEnabled(ProfilerArea.CPU, true);
        Profiler.SetAreaEnabled(ProfilerArea.Rendering, true);
        Profiler.SetAreaEnabled(ProfilerArea.Memory, true);
#endif
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateManager()
    {
        new GameObject("ProfilerManager").AddComponent<ProfilerManager>();
    }

    private bool _isDuplicate;
    
    private void Awake()
    {
        if (Instance != null)
        {
            _isDuplicate = true;
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (_isDuplicate)
            return;
        
#if !UNITY_EDITOR && DEVELOPMENT_BUILD && PROFILER_ENABLED
        Profiler.SetAreaEnabled(ProfilerArea.CPU, false);
        Profiler.SetAreaEnabled(ProfilerArea.Rendering, false);
        Profiler.SetAreaEnabled(ProfilerArea.Memory, false);
        Profiler.enabled = false;
#endif
    }
}
