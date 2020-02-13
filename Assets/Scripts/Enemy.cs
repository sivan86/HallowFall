using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    protected float speed;
    public bool destroyed = false;
    public string removerName;
    protected bool inventoryExit = false;
    protected Animator _anim;
    public bool hit;


    [SerializeField]
    private AudioClip audioHitEnemy;

    // Update is called once per frame
    void FixedUpdate ()
    {
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        speed = game.gameSpeed;

        //transform.Translate(Vector2.up * speed * Time.deltaTime);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.MovePosition(rb.position + Vector2.up * speed * Time.fixedDeltaTime);

        if (transform.position.y > 11.0f)
        {
            destroyed = true;
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "GameZone")
        {
            inventoryExit = true;
        }
        if (inventoryExit)
        {
            if (other.gameObject.tag == removerName)
            {
                if (other.gameObject.GetComponent<DragAndDrop>().drop == true)
                {
                    //Debug.Log(other.gameObject.GetComponent<DragAndDrop>().hitEnemy);
                    if (other.gameObject.GetComponent<DragAndDrop>().hitEnemy == false)
                    {
                        other.gameObject.GetComponent<DragAndDrop>().hitEnemy = true;
                        hit = true;
                        _anim.SetBool("Hit", hit);
                        AudioSource.PlayClipAtPoint(audioHitEnemy, Camera.main.transform.position, 1f);
                        Game game = GameObject.Find("GameManager").GetComponent<Game>();
                        //game.SpawnGoods(transform.position); // use this if I want to spawn candies and potion after enemy dies
                        game.SpawnArrows(transform.position); // use this if I want to spawn arrows after enemy dies
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }

    
}
