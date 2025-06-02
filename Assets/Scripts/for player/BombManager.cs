using UnityEngine;
using System.Collections.Generic;

public class BombManager : MonoBehaviour
{
    public static BombManager Instance;
    public GameObject bombIconPrefab;
    public Transform bombPanel;
    public int maxBombs = 5;

    private int currentBombs;
    private List<GameObject> bombIcons = new List<GameObject>();

    public AudioClip reloadClip; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentBombs = maxBombs;
        InitializeUI();
    }

    private void InitializeUI()
    {
        foreach (Transform child in bombPanel)
        {
            Destroy(child.gameObject);
        }
        bombIcons.Clear();

        for (int i = 0; i < maxBombs; i++)
        {
            GameObject icon = Instantiate(bombIconPrefab, bombPanel);
            bombIcons.Add(icon);
        }
    }

    public bool TryUseBomb()
    {
        if (currentBombs > 0)
        {
            currentBombs--;
            bombIcons[currentBombs].SetActive(false);
            return true;
        }
        return false;
    }

    public void ReloadBombs()
    {
        currentBombs = maxBombs;
        foreach (var icon in bombIcons)
            icon.SetActive(true);

        if (reloadClip != null)
            AudioSource.PlayClipAtPoint(reloadClip, Camera.main.transform.position);
    }
}