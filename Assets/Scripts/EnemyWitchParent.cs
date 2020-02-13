using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWitchParent : MonoBehaviour
{
    private float speed = 5.0f;
    void FixedUpdate()
    {
        //transform.Translate(Vector2.up * speed * Time.deltaTime);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.MovePosition(rb.position + Vector2.up * speed * Time.fixedDeltaTime);

        if (transform.position.y > 11.0f)
        {
            //destroyed = true;
            Destroy(gameObject);
        }
    }
}
