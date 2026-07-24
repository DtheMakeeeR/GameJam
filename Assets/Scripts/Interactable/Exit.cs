using UnityEngine;

public class Exit : Interactble
{
    [SerializeField] private AudioClip _exitSound;
    public override void MakeInteraction(PlayerController player)
    {
        SFXManager.Instance.PlaySoundOnce(_exitSound);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit;
#endif
    }
}
