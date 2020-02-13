using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private Vector2 targetPos;
    [SerializeField]
    private float Xincrement;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rightBorder;
    [SerializeField]
    private float leftBorder;
    [SerializeField]
    public int lives;
    private int candiesCollected = 0;
    public GameObject candyEffect;
    public GameObject effect;
    public GameObject enemyEffect;
    public GameObject arrowEffect;
    public Text livesDisplay;
    public float move;
    public Animator _anim;
    private SpriteRenderer _sprite;
    public bool hit;

    private int speedPowerup = 0;
    private int slowPowerup = 0;
    private int shieldPowerup = 0;
    public GameObject shieldGO;
    public Animator shieldEndAnim;
    [SerializeField]
    private GameObject timeBar;
    [SerializeField]
    private Animator timeBarAnim;
    [SerializeField]
    private Animator leftImageColor;
    [SerializeField]
    private Animator rightImageColor;

    private int lane = 1;
    private float[] xLanes = {-3.5f, 0.0f, 3.5f};

    [SerializeField]
    private AudioClip audioHitPlayer;
    [SerializeField]
    private AudioClip audioGameOver;
    [SerializeField]
    private AudioClip audioHitEnemy;
    [SerializeField]
    private AudioClip audioTurn;
    [SerializeField]
    private AudioClip audioPowerup;
    [SerializeField]
    private AudioClip audioCandy;
    [SerializeField]
    private GameObject gameOverAnim;

    private Vector3 startTouch;

    private void Start()
    {
        targetPos = transform.position;
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        //audioHitPlayer = GetComponent<AudioSource>();
        
    }

    void Update ()
    {
        SwipeMovement();
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        move = Mathf.Sign(targetPos.x - transform.position.x);
        _anim.SetFloat("Move", Mathf.Abs(targetPos.x - transform.position.x));
        //Debug.Log(targetPos.x - transform.position.x);

        if (move>0)
        {
            Flip(true);
        }
        else if (move < 0)
        {
            Flip(false);
        }

        livesDisplay.text = lives.ToString();

        if (lives <= 0)
        {
            Game game = GameObject.Find("GameManager").GetComponent<Game>();
            game.gameOver = true;
            game.continueScreenOn = true;
            //livesDisplay.enabled = false;
            AudioSource.PlayClipAtPoint(audioGameOver, Camera.main.transform.position, 0.4f);
            //Destroy(gameObject);
            gameObject.SetActive(false);
            gameOverAnim.transform.position = new Vector2(transform.position.x, gameOverAnim.transform.position.y); 
            gameOverAnim.SetActive(true);
        }
    }

    void SwipeMovement()
    {
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        if (Input.GetMouseButtonDown(0))
        {            
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (pos.y > -6)
                startTouch = pos;
            else
                startTouch = Vector3.zero;
        }
        if (Input.GetMouseButtonUp(0) && startTouch != Vector3.zero)
        {
            Vector3 swipeDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - startTouch;
            if (swipeDelta.magnitude > 0.5f)
            {
                if (swipeDelta.y < swipeDelta.x && swipeDelta.y > -swipeDelta.x && game.rightCollected>0)
                {
                    MoveRight();
                    
                    
                }
                if (swipeDelta.y < -swipeDelta.x && swipeDelta.y > swipeDelta.x && game.leftCollected>0)
                {
                    MoveLeft();
                    
                    
                }
            }
        }
    }

    void MoveRight()
    {
        if (lane <2)
        {
            lane = lane + 1;
            Game game = GameObject.Find("GameManager").GetComponent<Game>();
            game.rightCollected--;
            rightImageColor.SetTrigger("swipe");
            AudioSource.PlayClipAtPoint(audioTurn, Camera.main.transform.position, 0.4f);
            targetPos = new Vector2(xLanes[lane], transform.position.y);
        }   
    }

    void MoveLeft()
    {
        if (lane >0)
        {
            lane = lane - 1;
            Game game = GameObject.Find("GameManager").GetComponent<Game>();
            game.leftCollected--;
            leftImageColor.SetTrigger("swipe");
            AudioSource.PlayClipAtPoint(audioTurn, Camera.main.transform.position, 0.4f);
            targetPos = new Vector2(xLanes[lane], transform.position.y);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Right")
        {
            if (other.gameObject.GetComponent<DragAndDrop>().drop == true)
            {
                MoveRight();
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.tag == "Left")
        {
            if (other.gameObject.GetComponent<DragAndDrop>().drop == true)
            {
                MoveLeft();
                Destroy(other.gameObject);
            }
        }
        
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Right" && lane < 2)
        {
            if (other.gameObject.GetComponent<DragAndDrop>().drop == true)
            {
                lane = lane + 1;
                Destroy(other.gameObject);
            }       
        }

        if (other.gameObject.tag == "Left" && lane > 0)
        {
            if (other.gameObject.GetComponent<DragAndDrop>().drop == true)
            {
                lane = lane - 1;
                Destroy(other.gameObject);
            }   
        }

        targetPos = new Vector2(xLanes[lane], transform.position.y);

        if (other.gameObject.tag == "Enemy" && shieldPowerup==0)
        {
            if (other.gameObject.GetComponent<Enemy>().hit == false) //this is for the player won't get hit while the animation of the enemy fades out
            {
                lives--;
                _anim.SetTrigger("HitTrigger");
                AudioSource.PlayClipAtPoint(audioHitPlayer, Camera.main.transform.position, 0.4f);
                Instantiate(effect, new Vector2(transform.position.x, transform.position.y -2), Quaternion.identity);
            }    
        }
        else if (other.gameObject.tag == "Enemy" && shieldPowerup >0)
        {
            if (other.gameObject.GetComponent<Enemy>().hit == false)
            {
                AudioSource.PlayClipAtPoint(audioHitEnemy, Camera.main.transform.position, 0.4f);
                Instantiate(enemyEffect, other.gameObject.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
            }
        }

            if (other.gameObject.tag == "Slow")
        {
            slowPowerup++;
            Game game = GameObject.Find("GameManager").GetComponent<Game>();
            game.slowPowerup = true;
            AudioSource.PlayClipAtPoint(audioPowerup, Camera.main.transform.position, 0.4f);
            //Debug.Log(game.gameSpeed);
            timeBar.SetActive(true);
            timeBarAnim.Play(0);
            StartCoroutine(SlowPowerUpRoutine());
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Speed")
        {
            speedPowerup++;
            Game game = GameObject.Find("GameManager").GetComponent<Game>();
            game.speedPowerup = true;
            AudioSource.PlayClipAtPoint(audioPowerup, Camera.main.transform.position, 0.4f);
            timeBar.SetActive(true);
            timeBarAnim.Play(0);
            StartCoroutine(SpeedPowerUpRoutine());
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Shield")
        {  
            shieldPowerup ++;
            shieldGO.SetActive(true);
            AudioSource.PlayClipAtPoint(audioPowerup, Camera.main.transform.position, 0.4f);
            shieldEndAnim.Play("Shield",0,0);
            StartCoroutine(ShieldPowerUpRoutine());
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Candy")
        {
            
            Game game = GameObject.Find("GameManager").GetComponent<Game>();
            game.candiesCollected++;
            //game.candiesCollected = candiesCollected;
            AudioSource.PlayClipAtPoint(audioCandy, Camera.main.transform.position, 0.4f);

            Destroy(other.gameObject.GetComponent<CapsuleCollider2D>());
            //Instantiate(candyEffect, other.gameObject.transform.position, Quaternion.identity);
            //Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "LeftCollect")
        {
            Game game = GameObject.Find("GameManager").GetComponent<Game>();
            game.leftCollected ++;
            AudioSource.PlayClipAtPoint(audioCandy, Camera.main.transform.position, 0.4f);
            //Instantiate(arrowEffect, other.gameObject.transform.position, Quaternion.identity);
            //Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "RightCollect")
        {
            Game game = GameObject.Find("GameManager").GetComponent<Game>();
            game.rightCollected++;
            AudioSource.PlayClipAtPoint(audioCandy, Camera.main.transform.position, 0.4f);
            //Instantiate(arrowEffect, other.gameObject.transform.position, Quaternion.identity);
            //Destroy(other.gameObject);
        }
    }

    void Flip(bool faceRight)
    {
        if (faceRight==true)
        {
            _sprite.flipX = false;
        }
        else if (faceRight==false)
        {
            _sprite.flipX = true;
        }
    }

    public IEnumerator SlowPowerUpRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        slowPowerup--;
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        if (slowPowerup==0)
            game.slowPowerup = false;
    }

    public IEnumerator SpeedPowerUpRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        speedPowerup--;
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        if (speedPowerup==0)
            game.speedPowerup = false;
    }

    public IEnumerator ShieldPowerUpRoutine()
    {
        yield return new WaitForSeconds(7.5f);
        
        if (shieldPowerup==1)
        {      
            shieldEndAnim.SetTrigger("end");
        }
        yield return new WaitForSeconds(2.5f);
        shieldPowerup--;
        if (shieldPowerup==0)
            shieldGO.SetActive(false);
    }
}
