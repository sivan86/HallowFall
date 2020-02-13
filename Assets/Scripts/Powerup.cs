using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    [SerializeField]
    private float powerupSpeed;
    private float speed;
    public bool destroyed = false;

    void FixedUpdate()
    {
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        speed = powerupSpeed * game.gameSpeed;

        //transform.Translate(Vector2.up * speed * Time.deltaTime);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.MovePosition(rb.position + Vector2.up * speed * Time.fixedDeltaTime);

        if (transform.position.y > 11.0f)
        {
            destroyed = true;
            Destroy(gameObject);
        }
    }
}
