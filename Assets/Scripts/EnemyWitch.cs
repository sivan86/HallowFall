using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWitch : Enemy {

    [SerializeField]
    private float sidesSpeed=3.0f;
    private SpriteRenderer _sprite;
    private Vector2 targetPos;
    private float enemyPosX;

    void Update ()
    {

        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        speed = game.gameSpeed;

        if (transform.localPosition.x == 3.5)
        {
            targetPos.x = -3.5f;
            _sprite.flipX = false;
        }
        if (transform.localPosition.x == -3.5)
        {
            targetPos.x = 3.5f;
            _sprite.flipX = true;
        }
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, sidesSpeed * Time.deltaTime);
    }

    private void Start()
    {
        enemyPosX = Random.Range(-3.0f, 3.5f);
        transform.position = new Vector2(enemyPosX, transform.position.y);

        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        targetPos = new Vector2(-3.5f, transform.localPosition.y);
    }    
}
