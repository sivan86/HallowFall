using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudColor : MonoBehaviour {

    private SpriteRenderer _sprite;
    private float alpha;
    private Color B;
    private float speed = 2.0f;

    private void Start()
    {        
        _sprite = GetComponent<SpriteRenderer>();
        alpha = _sprite.color.a;
        B = new Color(102.0f/255.0f, 102.0f/255.0f, 102.0f/255.0f, alpha);
    }

    private void Update()
    {
        ChangeCloudColor();
    }

    public void ChangeCloudColor()
    {
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        if (game.level ==3)
             _sprite.color = Color.Lerp(_sprite.color, B, Mathf.PingPong(Time.time * speed, 1.0f));
    }
	

}
