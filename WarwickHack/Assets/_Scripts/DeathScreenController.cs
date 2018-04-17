using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour {

    public Text gameOverText;
    private Image deathScreen;

    private void Start()
    {
        deathScreen = GetComponent<Image>();
        Color c = deathScreen.color;
        c.a = 0;
        deathScreen.color = c;

        c = gameOverText.color;
        c.a = 0;
        gameOverText.color = c;
    }

    public void PlayerDeath()
    {
        deathScreen.DOFade(1, 3);
        gameOverText.DOFade(1, 3);
        StartCoroutine(LoadTitle());
    }

    IEnumerator LoadTitle()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync("Title");
    }
}
