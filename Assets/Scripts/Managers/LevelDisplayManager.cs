using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelDisplayManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI taskText;
    [SerializeField] private TextMeshProUGUI shadowText;

    private bool _isShadowLevel = false;

    void Start()
    {
        shadowText.enabled = false;
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        levelText.text = (9 - sceneIndex + 1) + " days left";

        switch (sceneIndex)
        {
            case 1:
                taskText.text = "Find a note from the diary";
                break;
            case 2:
                taskText.text = "Find the key and get out";
                break;
            case 3:
                taskText.text = "Find your brother's toy and bring it to him";
                break;
            case 4:
                taskText.text = "Find your brother, the key, and get out";
                break;
            case 5:
                taskText.text = "Find your brother before SHE does";
                _isShadowLevel = true;
                shadowText.enabled = true;
                break;
            case 6:
                taskText.text = "Find the key and get out";
                break;
            case 7:
                taskText.text = "Find your brother before SHE does";
                _isShadowLevel = true;
                shadowText.enabled = true;
                break;
            case 8:
                taskText.text = "Find the lamplighter and get out";
                break;
            case 9:
                taskText.text = "Find yourself";
                break;
        }
    }

    void Update()
    {
        if (_isShadowLevel && Shadow.Instance != null)
        {
            if (Shadow.Instance.timeOut > 0)
            {
                shadowText.text = "Shadow will start moving after: " + Shadow.Instance.timeOut;
            }
            else
            {
                shadowText.text = "Shadow is coming!";
            }
        }
    }
}