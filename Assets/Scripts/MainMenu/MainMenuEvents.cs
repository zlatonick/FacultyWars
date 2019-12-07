using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    public void onPlayButtonPressed()
    {
        Debug.Log("Play button pressed");
        SceneManager.LoadScene("ChooseFaculty");
    }
}
