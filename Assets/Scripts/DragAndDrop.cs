using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour {

    float distance = 10;
    [SerializeField]
    private float speed;
    
    public Text debugText;

    public bool drop = false;
    public bool hitEnemy = false;

    private SpriteRenderer sprite;
    private Vector3 cardFirstPos;
    private Vector3 cardEndPos;
    private float touchTimeStart;
    private float touchTimeFinish;
    [SerializeField]
    private float throwForce = 0.3f;
    private Vector3 objPos;
    private Vector3 mouseFirstPos;
    private Camera cam;

    private AudioSource audioTrow;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        //debugText = GameObject.Find("Canvas/debug").GetComponent<Text>();
        audioTrow = GetComponent<AudioSource>();
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        game.mouseUp = false;
    }

    private void FixedUpdate()
    {
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        speed = game.gameSpeed;

        if (drop)
        {
            
            //transform.Translate(Vector2.up * speed * Time.deltaTime);
            //GetComponent<Rigidbody2D>().MovePosition(transform.position + Vector3.up * speed * Time.fixedDeltaTime);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * speed / rb.mass * rb.drag);

            if (sprite)
            {
                sprite.sortingOrder = 0;
            }

            if (transform.position.y > 11.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        if (!drop)
        {
            cardFirstPos = transform.position;
            mouseFirstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchTimeStart = Time.time;            
        }

    }

    private void OnMouseDrag()
    {
        if (!drop)
        {     
            //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            //objPos = Camera.main.ScreenToWorldPoint(mousePos);
            //transform.position = objPos;            
            transform.position = cardFirstPos + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseFirstPos);
        }        
    }

    public void OnMouseUp()
    {
        if (!drop && (transform.position - cardFirstPos).magnitude > 0.3f)
        {
            drop = true;

            cardEndPos = transform.position;
            touchTimeFinish = Time.time;

            Swipe();
            audioTrow.Play();

            Game game = GameObject.Find("GameManager").GetComponent<Game>();
            game.SpawnToInventory(cardFirstPos);
            game.mouseUp = true;
        }
        else if (!drop)
            transform.position = cardFirstPos;
    }

    void SnapToLine()
    {
        Vector3 targetPose = transform.position;
        if (objPos.x > -1.75 && objPos.x < 1.75)
            targetPose = new Vector3(0f, transform.position.y, transform.position.z);
        else if (objPos.x >= 1.75)
            targetPose.x = 3.5f;
        else if (objPos.x <= -1.75)
            targetPose.x = -3.5f;

        transform.position = Vector3.MoveTowards(transform.position, targetPose, 5 * Time.deltaTime);
    }

    void Swipe()
    {
        Vector3 swipeDirection = cardEndPos - cardFirstPos;
        float timeInterval = touchTimeFinish - touchTimeStart;
        Debug.Log("Time: " + timeInterval + " Distance: " + swipeDirection.magnitude);
        //debugText.text = "Time: " + timeInterval.ToString("0.00") + 
        //    " Distance: " + swipeDirection.magnitude.ToString("0.00") + 
        //    " Ratio:" + (swipeDirection.magnitude/timeInterval).ToString("0.00");
        GetComponent<Rigidbody2D>().AddForce(swipeDirection / timeInterval * throwForce, ForceMode2D.Impulse);
    }
}
