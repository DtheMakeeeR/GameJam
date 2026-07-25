using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFlagsManager : MonoBehaviour
{
    public static LevelFlagsManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField] private int _flags = 1;

    int Flags
    {
        get => _flags;
        set
        {            
            _flags = value;
            if (_flags <= 0)
            {
                LoadNextLevel();
            }
        }
    }

    private void LoadNextLevel()
    {
        int curIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(curIndex + 1);
    }

    public void DecreaseFlags(int amount = 1) => Flags -= amount;
}
