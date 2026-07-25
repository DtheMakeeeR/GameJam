using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI taskText;

    void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        levelText.text = (9 -  sceneIndex + 1) + " days left";

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
                break;
            case 6:
                taskText.text = "Find the key and get out";
                break;
            case 7:
                taskText.text = "Find your brother before SHE does";
                break;
            case 8:
                taskText.text = "Find the lamplighter and get out";
                break;
            case 9:
                taskText.text = "Find yourself";
                break;
        }
    }
}