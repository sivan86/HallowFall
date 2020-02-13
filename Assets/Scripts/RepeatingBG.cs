using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBG : MonoBehaviour {

    public float cloudSpeed;
    private float speed;

    public float endY;
    public float startY;

	void Update ()
    {
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        speed = cloudSpeed * game.gameSpeed;

        transform.Translate(Vector2.up * speed * Time.deltaTime);
        
        if (transform.position.y >= endY)
        {
            Vector2 pos = new Vector2(transform.position.x, startY);
            transform.position = pos;
        }
	}
}
