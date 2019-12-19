using MetaInfo;
using Preparing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    public GameObject chooser;

    public void OpenChooser()
    {
        chooser.SetActive(true);
    }

    public void StartMatch(int facultyId)
    {
        switch (facultyId)
        {
            case 0:
                StuffPack.stuffClass = StuffClass.IASA;
                PreparingData.cardCount = 10;
                break;

            case 1:
                StuffPack.stuffClass = StuffClass.FICT;
                PreparingData.cardCount = 17;
                break;

            case 2:
                StuffPack.stuffClass = StuffClass.FPM;
                PreparingData.cardCount = 19;
                break;
        }

        SceneManager.LoadScene("Preparing");
    }
}
