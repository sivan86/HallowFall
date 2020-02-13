using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour {

    [SerializeField]
    private float candySpeed;
    private float speed;
    public bool destroyed = false;
    private bool isCollided = false;
    [SerializeField]
    Vector3 targetposition;
    public GameObject effect;

    void FixedUpdate()
    {
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        speed = candySpeed * game.gameSpeed;

        
        if (isCollided==false)
        {
            //transform.Translate(Vector2.up * speed * Time.deltaTime);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.MovePosition(rb.position + Vector2.up * speed * Time.fixedDeltaTime);
        }
        else
        {
           //Vector3 targetposition = new Vector3(-4.65f, 7.55f, 0f);
            transform.position = Vector3.MoveTowards(transform.position, targetposition, 9*Time.deltaTime);
            if (transform.position.y >= targetposition.y)
                Destroy(gameObject);
        }
        //Debug.Log(isCollided);

        if (transform.position.y > 11.0f)
        {
            destroyed = true;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isCollided = true;
            Instantiate(effect, transform.position, Quaternion.identity,transform);
        }
    }
}
