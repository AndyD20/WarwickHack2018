using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;

	public void PlayButton()
    {
        foreach(GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

        foreach(GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }

    public void GoBack()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(true);
        }
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SelectDifficulty(int difficulty)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
        SceneManager.LoadSceneAsync("_Scenes/Level1");
    }

    
}
