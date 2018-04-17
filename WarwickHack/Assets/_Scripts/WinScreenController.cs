using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class WinScreenController : MonoBehaviour {

    public Transform winText;
    public Transform menuButton;
    public Transform baseScore;
    public Transform timeTaken;
    public Transform heartsRemaining;
    public Transform totalScore;

    public PlayerPlatformerController player;
    public Timer timer;

    public Transform[] otherScreenItems;

    private Image winScreen;
    private int finalScore = 0;
    private string totalScoreText;
    private string timeTakenText;
    private string heartsRemainingText;

    private int timeSeconds;
    private int noOfRemainingHearts;
    

	// Use this for initialization
	void Start () {
        winScreen = transform.GetComponent<Image>();
        Color c = winScreen.color;
        c.a = 0;
        winScreen.color = c;

        winText.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        baseScore.gameObject.SetActive(false);
        timeTaken.gameObject.SetActive(false);
        heartsRemaining.gameObject.SetActive(false);
        totalScore.gameObject.SetActive(false);

        foreach (Transform t in otherScreenItems)
        {
            t.gameObject.SetActive(false);
        }

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1))
        {
            WinLevel();
        }
	}

    public void WinLevel()
    {
        timeSeconds = timer.Seconds;
        noOfRemainingHearts = player.CurrentHealth;
        


        baseScore.GetComponent<Text>().text = string.Format("Base Score: {0:0000}", player.PlayerScore);

        timeTakenText = "       Time Taken: {0:0000} seconds {1}(1 point per second under 5 minutes)";
        timeTaken.GetComponent<Text>().text = string.Format(timeTakenText, timeSeconds, System.Environment.NewLine);

        heartsRemainingText = " Hearts Remaning: {0} {1}(100 points per heart)";
        heartsRemaining.GetComponent<Text>().text = string.Format(heartsRemainingText, noOfRemainingHearts, System.Environment.NewLine);

        totalScoreText = "";

        switch (PlayerPrefs.GetInt("difficulty", 1))
        {
            case 1:
                totalScoreText = "   Total Score: {0:0000} {1}(/2 for easy difficulty) ";
                break;
            case 2:
                totalScoreText = "   Total Score: {0:0000} ";
                break;
            case 4:
                totalScoreText = "   Total Score: {0:0000} {1}(*2 for hard difficulty) ";
                break;
            default:
                totalScoreText = "   Total Score: {0:0000} ";
                break;
        }
        
        totalScore.GetComponent<Text>().text = string.Format(totalScoreText, 0, System.Environment.NewLine);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        
        
        StartCoroutine(FadeInBackground());
    }

    IEnumerator FadeInBackground()
    {
        yield return new WaitForSeconds(1);
        winScreen.DOFade(1, 3);
        yield return new WaitForSeconds(3);

        float seconds = winText.GetComponent<Text>().text.ToCharArray().Length * 0.05f + 0.5f;
        winText.gameObject.SetActive(true);
        StartCoroutine(WriteInCharacters(winText.GetComponent<Text>()));
        yield return new WaitForSeconds(seconds);

        seconds = baseScore.GetComponent<Text>().text.ToCharArray().Length * 0.05f + 0.5f;
        baseScore.gameObject.SetActive(true);
        StartCoroutine(WriteInCharacters(baseScore.GetComponent<Text>()));
        yield return new WaitForSeconds(seconds);

        seconds = timeTaken.GetComponent<Text>().text.ToCharArray().Length * 0.05f + 0.5f;
        timeTaken.gameObject.SetActive(true);
        StartCoroutine(WriteInCharacters(timeTaken.GetComponent<Text>()));
        yield return new WaitForSeconds(seconds);

        seconds = heartsRemaining.GetComponent<Text>().text.ToCharArray().Length * 0.05f + 0.5f;
        heartsRemaining.gameObject.SetActive(true);
        StartCoroutine(WriteInCharacters(heartsRemaining.GetComponent<Text>()));
        yield return new WaitForSeconds(seconds);

        seconds = totalScore.GetComponent<Text>().text.ToCharArray().Length * 0.05f + 0.5f;
        totalScore.gameObject.SetActive(true);
        StartCoroutine(WriteInCharacters(totalScore.GetComponent<Text>()));
        yield return new WaitForSeconds(seconds);

        seconds = menuButton.GetComponent<Text>().text.ToCharArray().Length * 0.05f + 0.5f;
        menuButton.gameObject.SetActive(true);
        StartCoroutine(WriteInCharacters(menuButton.GetComponent<Text>()));
        yield return new WaitForSeconds(seconds);

        foreach (Transform t in otherScreenItems)
        {
            seconds = t.GetComponent<Text>().text.ToCharArray().Length * 0.05f + 0.5f;
            t.gameObject.SetActive(true);
            StartCoroutine(WriteInCharacters(t.GetComponent<Text>()));
            yield return new WaitForSeconds(seconds);
        }

        int scoreVariable = player.PlayerScore;

        if (scoreVariable > 0)
        {
            DOTween.To(() => scoreVariable, x => scoreVariable = x, 0, 2).OnUpdate(() => UpdateTextElement(baseScore.GetComponent<Text>(), "Base Score: {0:0000}", scoreVariable));
            DOTween.To(() => finalScore, x => finalScore = x, scoreVariable, 2).OnUpdate(() => UpdateTextElement(totalScore.GetComponent<Text>(), totalScoreText, finalScore));
            yield return new WaitForSeconds(2.5f);
        }

        
        scoreVariable = 300 - timeSeconds;
        DOTween.To(() => timeSeconds, x => timeSeconds = x, 0, 2).OnUpdate(() => UpdateTextElement(timeTaken.GetComponent<Text>(), timeTakenText, timeSeconds));
        DOTween.To(() => finalScore, x => finalScore = x, finalScore + scoreVariable, 2).OnUpdate(() => UpdateTextElement(totalScore.GetComponent<Text>(), totalScoreText, finalScore));
        yield return new WaitForSeconds(2.5f);


        scoreVariable = noOfRemainingHearts * 100;
        DOTween.To(() => noOfRemainingHearts, x => noOfRemainingHearts = x, 0, 2).OnUpdate(() => UpdateTextElement(heartsRemaining.GetComponent<Text>(), heartsRemainingText, noOfRemainingHearts));
        DOTween.To(() => finalScore, x => finalScore = x, finalScore + scoreVariable, 2).OnUpdate(() => UpdateTextElement(totalScore.GetComponent<Text>(), totalScoreText, finalScore));
        yield return new WaitForSeconds(2.5f);



        switch (PlayerPrefs.GetInt("difficulty", 1))
        {
            case 1:
                scoreVariable = finalScore / 2;
                DOTween.To(() => finalScore, x => finalScore = x, scoreVariable, 2).OnUpdate(() => UpdateTextElement(totalScore.GetComponent<Text>(), totalScoreText, finalScore));
                break;
            case 2:
                break;
            case 4:
                scoreVariable = finalScore * 2;
                DOTween.To(() => finalScore, x => finalScore = x, scoreVariable, 2).OnUpdate(() => UpdateTextElement(totalScore.GetComponent<Text>(), totalScoreText, finalScore));
                break;
        }
    }

    IEnumerator WriteInCharacters(Text textToWrite)
    {
        var chars = textToWrite.text.ToCharArray();
        string s = "";
        textToWrite.text = s;
        winText.gameObject.SetActive(true);
        foreach (char c in chars)
        {
            s += c;
            textToWrite.text = s;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void UpdateTextElement(Text textField, string text, int value)
    {
        textField.text = string.Format(text, value, System.Environment.NewLine);
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("Title");
    }
}
