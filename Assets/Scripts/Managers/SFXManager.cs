using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    [SerializeField] private AudioSource _audioSource;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PlaySoundOnce(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
