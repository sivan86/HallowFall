using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {


    [SerializeField]
    private GameObject[] CardInstance;
    [SerializeField]
    private GameObject[] enemiesPatterns;
    [SerializeField]
    private GameObject[] powerupsPatterns;
    [SerializeField]
    private GameObject[] candiesPatterns;
    [SerializeField]
    private GameObject levelUpAnim;
    private bool levelAnimationPlayed = false;
    [SerializeField]
    private GameObject[] clouds;

    [SerializeField]
    private GameObject[] goods;
    public int potions;

    public bool gameOver = false;

    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject continuePanel;
    public bool continueScreenOn = false;

    [SerializeField]
    private GameObject[] arrowsToCollect;
    public Text leftCollectedText;
    public int leftCollected = 5;
    public Text rightCollectedText;
    public int rightCollected = 5;

    public Text counterText;
    public int score;
    public Text candiesCollectedText;
    public int candiesCollected = 0;
    public float gameSpeed = 5;
    public bool mouseUp;
    private List<int> cardsSequence = new List<int>();

    public int levelTime = 0;
 
    private float gameTimer;
    [SerializeField]
    private GameObject player;

    public int totalCandies = 0;

    public bool slowPowerup = false;
    public bool speedPowerup = false;
    //public bool shieldPowerup = false;

    public int level = 0;
    private int nLevelCards = 4;
    private int nLevelPatterns = 3;
    public int nLevelPowerups = 0;
    public int nLevelEnemies = 1;
    public int nLevelCandies = 3;
    private float leveEnemySpacing;
    private float levelGameSpeed = 5;
    [SerializeField]
    private GameObject[] levelBG;

    [SerializeField]
    private GameObject fadeInPanel;

    [SerializeField]
    private GameObject tutorialArrow;
    [SerializeField]
    private GameObject tutorialSun;

    void Start () {
        //PlayerPrefs.DeleteAll();
        fadeInPanel.SetActive(true);
        gameTimer = 0;

        nLevelEnemies = 1;

        cardsSequence.Add(-1);
        cardsSequence.Add(-1);
        Vector3 cardPosition = new Vector3(-3.9f, -7.85f, 0f);

        for (int i = 0; i <= 3; i++) 
        {
            int j = i;
            if (i == 0)
                j = 2;
            else if (i == 1)
                j = 3;

            SpawnToInventoryTutorial(cardPosition, j); // If I want to use right and left arrow cards then I should write i instead j and delete j
            cardPosition.x = cardPosition.x + 2.6f;
        }
        //SpawnToInventoryTutorial(new Vector3(-1.3f, -7.85f, 0f));
        //SpawnToInventoryTutorial(new Vector3(1.3f, -7.85f, 0f));
        //SpawnToInventoryTutorial(new Vector3(3.9f, -7.85f, 0f));
        level = 0;
        gameSpeed = 5;
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnCandiesRoutine());

        if (!PlayerPrefs.HasKey("Potions"))
            PlayerPrefs.SetInt("Potions", 3);
        potions = PlayerPrefs.GetInt("Potions");
    }

    private void Update()
    {
        if (!gameOver)
        {
            //counterText.text = ((int)(10 * Time.timeSinceLevelLoad)).ToString();
            //score = (int)(10 * Time.timeSinceLevelLoad);
            gameTimer += Time.deltaTime;

            score = (int)(10 * gameTimer);

        }
        counterText.text = ((int)(10 * gameTimer)).ToString();
        candiesCollectedText.text = candiesCollected.ToString();
        leftCollectedText.text = leftCollected.ToString();
        rightCollectedText.text = rightCollected.ToString();

        if (gameOver)
        {                        
            if (continueScreenOn)
            {
                if (PlayerPrefs.GetFloat("Highscore") < score)
                    PlayerPrefs.SetFloat("Highscore", score);
                if (PlayerPrefs.HasKey("TotalCandies"))
                    totalCandies = PlayerPrefs.GetInt("TotalCandies");
                totalCandies = totalCandies + candiesCollected;
                PlayerPrefs.SetInt("TotalCandies", totalCandies);
                //candiesCollected = 0;
                StartCoroutine(LoadContinueScreen());
                continueScreenOn = false; // I set it to false because I need this to run only once
            }                
            //Destroy(levelUpAnim);
        }

        Tutorial();

        Levels();

        powerUps();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIbuttons button = GameObject.Find("Canvas").GetComponent<UIbuttons>();
            button.MainMenuButton();
        }
    }

    void SpawnToInventoryTutorial(Vector3 inventoryPosition, int card)
    {
        GameObject go = Instantiate(CardInstance[card], inventoryPosition, Quaternion.identity);
        go.transform.GetChild(0).gameObject.SetActive(true);
    }

    // this function is called on DragAndDrop Class
    public void SpawnToInventory(Vector3 inventoryPosition) 
    {
        int randomNum = Random.Range(2, nLevelCards); // If I want to use right and left arrow cards then random should start at 0 and not at 2
        while (cardsSequence[0] == randomNum && cardsSequence[1] == randomNum)
            randomNum = Random.Range(2, nLevelCards); // If I want to use right and left arrow cards then random should start at 0 and not at 2

        cardsSequence.RemoveAt(0);
        cardsSequence.Add(randomNum);

        GameObject go = Instantiate(CardInstance[randomNum], inventoryPosition, Quaternion.identity);
        
    }

    public IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            if (gameOver == false)
            {
                if (level > 0)
                {
                    leveEnemySpacing = Random.Range(2f, 2.5f);
                    if (level == 2)
                        leveEnemySpacing = Random.Range(1f, 2.5f);
                    if (level == 3)
                        leveEnemySpacing = Random.Range(1f, 2f);

                    int randomPattern = Random.Range(3, nLevelPatterns);
                    Instantiate(enemiesPatterns[randomPattern], transform.position, Quaternion.identity);
                }
                else if (level == 0)
                {
                    leveEnemySpacing = 3f;
                    int pattern = 2;
                    if ((int)(Time.timeSinceLevelLoad) > 3)
                        pattern = 1;
                    Instantiate(enemiesPatterns[pattern], transform.position, Quaternion.identity);
                }                
            }
            yield return new WaitForSeconds(leveEnemySpacing);
        }
    }

    public IEnumerator SpawnPowerupRoutine()
    {
        while (true)
        {
            if (gameOver == false)
            {
                if (level > 1)
                {
                    int randomPattern = Random.Range(0, nLevelPowerups);
                    Instantiate(powerupsPatterns[randomPattern], transform.position, Quaternion.identity);
                }
                
            }
            yield return new WaitForSeconds(Random.Range(5f, 8f));
        }
    }

    public IEnumerator SpawnCandiesRoutine()
    {
        while (true)
        {
            if (gameOver == false)
            {
                int randomPattern;

                for (int i = 0; i < Random.Range(3, 10); i++)
                {
                    randomPattern = Random.Range(0, 3);
                    Instantiate(candiesPatterns[randomPattern], transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(1f);
                }                
            }
            yield return new WaitForSeconds(Random.Range(1f, 8f));
        }       
    }

    private void powerUps()
    {
        if (slowPowerup)
            gameSpeed = levelGameSpeed - 2;
        else if (speedPowerup)
            gameSpeed = levelGameSpeed + 2;
        else
            gameSpeed = levelGameSpeed;
    }

    //public void powerUp(int i)
    //{
    //    float[] speeds = { 3, 5, 7 };
    //    gameSpeed = speeds[i];

    //    StartCoroutine();
    //}

    private void Tutorial()
    {
        // if I want to use arrows cards then I need to enable the TutorialArrow
        //if ((int)(Time.timeSinceLevelLoad) == 1)
        //    tutorialArrow.SetActive(true);
        if ((int)(Time.timeSinceLevelLoad) == 1)
            tutorialSun.SetActive(true);
        if (mouseUp)
        {
            //tutorialArrow.SetActive(false);
            tutorialSun.SetActive(false);
        }

        if ((int)(Time.timeSinceLevelLoad) == 3)
            nLevelEnemies = 2;
    }

    void Levels()
    {
        if (gameOver!=true)
        {
            if ((int)gameTimer == 10)
            {
                level = 1;
                nLevelCards = 4;
                nLevelPowerups = 0;
                nLevelPatterns = 6;
                nLevelEnemies = 2;
                levelGameSpeed = 6;
                //levelUpAnim.SetActive(true);
            }

            if ((int)gameTimer == 35)
            {
                level = 2;
                nLevelCards = 5;
                nLevelPowerups = 2;
                nLevelEnemies = 3;
                levelGameSpeed = 7;
                gameSpeed = levelGameSpeed;
                levelBG[0].SetActive(true);
                levelUpAnim.SetActive(true);
            }

            if ((int)gameTimer == 100)
            {
                level = 3;
                nLevelCards = 6;
                nLevelPowerups = 3;
                nLevelPatterns = 8;
                nLevelEnemies = 3;
                levelBG[1].SetActive(true);
                levelUpAnim.SetActive(true);
                //Instantiate(levelUpAnim, new Vector2(0f, 2.9f), Quaternion.identity);
                Destroy(clouds[0]);
                Destroy(clouds[1]);
            }
        }
    }

    //this function is called on Enemy class
    //public void SpawnGoods(Vector2 enemyPos)
    //{
    //    int randomNum = Random.Range(0, 50);
    //    if (randomNum == 30) //if randomNum=30 spawn potion
    //    {
    //        Instantiate(goods[0], enemyPos, Quaternion.identity);
    //        potions++;
    //    }

    //    if (randomNum > 5 && randomNum < 12) //spawn candy1
    //    {
    //        Instantiate(goods[1], enemyPos, Quaternion.identity);
    //        candiesCollected++;
    //    }

    //    if (randomNum > 35 && randomNum < 42) //spawn candy 2
    //    {
    //        Instantiate(goods[2], enemyPos, Quaternion.identity);
    //        candiesCollected++;
    //    }
    //}

    //this function is called on Enemy class
    public void SpawnArrows(Vector2 enemyPos)
    {
        int randomNum = Random.Range(0, 10);
        if (randomNum > 0 && randomNum < 4) //spawn left arrow
        {
            Instantiate(arrowsToCollect[0], enemyPos, Quaternion.identity);
            leftCollected++;
        }

        if (randomNum > 3 && randomNum < 7) //spawn right arrow
        {
            Instantiate(arrowsToCollect[1], enemyPos, Quaternion.identity);
            rightCollected++;
        }
    }

    IEnumerator LoadGameOverScreen()
    {
        yield return new WaitForSeconds(1.3f);
        gameOverPanel.SetActive(true);
    }

    IEnumerator LoadContinueScreen()
    {
        yield return new WaitForSeconds(1.3f);
        continuePanel.SetActive(true);
    }

    public void continueGame()
    {
        gameOver = false;
        player.SetActive(true);
        candiesCollected = 0;
        Player playerLives = GameObject.Find("Spider").GetComponent<Player>();
        playerLives.lives = 1;

    }
}
