using UnityEngine;
using UnityEngine.UI;

public class Letter : Interactble
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private bool _isFlag = true;
    private void Start()
    {
        _panel = GameObject.FindGameObjectWithTag("LetterPanel");
        _panel.SetActive(false);
        _panel.GetComponentInChildren<Button>()?.onClick.AddListener(OnDone);
    }
    public override void MakeInteraction(PlayerController player)
    {
        _panel.SetActive(true);
        player.SpecialFlag = false;
        transform.parent?.gameObject.SetActive(false);
    }
    public void OnDone()
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.SpecialFlag = true;
        if (_isFlag) LevelFlagsManager.Instance.DecreaseFlags();
    }
}
