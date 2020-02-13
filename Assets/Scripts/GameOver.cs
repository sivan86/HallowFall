using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public Text highScoreText;
    public Text scoreText;
    public Text candiesText;
    public int score;
    private float highScore;
    public int candies;
    private float animationCountTime = 0.8f;
    private float currentScore = 0f;
    private float currentHighScore = 0f;
    private float currentCandies = 0f;
    public Animator restartBtnOutAnim;
    public Animator homeBtnOutAnim;
    public Animator candyFieldOutAnim;
    public Animator highScoreFieldOutAnim;
    public Animator scoreFieldOutAnim;
    //public Animator bgOutAnim;
    public Animator bgTwoOutAnim;
    private string sceneName;
    [SerializeField]
    private AudioSource clickIn;
    [SerializeField]
    private AudioSource clickOut;

    private void Start()
    {
        clickIn.Stop();
        clickOut.Stop();
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        score = game.score;
        candies = game.candiesCollected;
        //Debug.Log(game.score);
        //scoreText.text = score.ToString();
        highScoreText.text = ((int)PlayerPrefs.GetFloat("Highscore")).ToString();
        candiesText.text = candies.ToString();
        highScore = (int)PlayerPrefs.GetFloat("Highscore");
    }

    public void RestartButton()
    {
        clickIn.Play();
        sceneName = "Game";
        StartCoroutine(GameOverScreenOut(sceneName));  
    }

    public void MainMenuButton()
    {
        clickOut.Play();
        sceneName = "MainMenu";
        StartCoroutine(GameOverScreenOut(sceneName));
    }

    //   void Update ()
    //   {
    //	if (Input.GetKeyDown(KeyCode.R)|| Input.GetMouseButtonDown(0))
    //       {
    //           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //       }
    //}

    private void Update()
    {                
        
        StartCoroutine(LoadScoreText());

        StartCoroutine(LoadHighScoreText());
        
        StartCoroutine(LoadCandyText());

        if (Input.GetKeyDown(KeyCode.Escape))
            MainMenuButton();
            //SceneManager.LoadScene("MainMenu");
    }

    IEnumerator LoadScoreText()
    {
        yield return new WaitForSeconds(1.0f);
        //scoreText.enabled = true;
        scoreText.color = new Color(1, 1, 1, 1);
        currentScore = Mathf.Clamp(currentScore + animationCountTime * Time.deltaTime * score, 0f, score);
        scoreText.text = currentScore.ToString("0");
    }

    IEnumerator LoadHighScoreText()
    {
        yield return new WaitForSeconds(1.2f);
        //candiesText.enabled = true;
        highScoreText.color = new Color(1, 1, 1, 1);
        currentHighScore = Mathf.Clamp(currentHighScore + animationCountTime * Time.deltaTime * highScore, 0f, highScore);
        highScoreText.text = currentHighScore.ToString("0");
    }

    IEnumerator LoadCandyText()
    {
        yield return new WaitForSeconds(1.4f);
        //candiesText.enabled = true;
        candiesText.color = new Color(1, 1, 1, 1);
        currentCandies = Mathf.Clamp(currentCandies + animationCountTime * Time.deltaTime * candies, 0f, candies);
        candiesText.text = currentCandies.ToString("0");
    }

    IEnumerator GameOverScreenOut(string sceneName)
    {
        restartBtnOutAnim.SetTrigger("end");
        homeBtnOutAnim.SetTrigger("end");
        candyFieldOutAnim.SetTrigger("end");
        highScoreFieldOutAnim.SetTrigger("end");
        scoreFieldOutAnim.SetTrigger("end");
        //bgOutAnim.SetTrigger("end");
        bgTwoOutAnim.SetTrigger("end");
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene(sceneName);
    }
}