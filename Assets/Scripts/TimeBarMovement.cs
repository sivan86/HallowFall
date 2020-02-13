using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBarMovement : MonoBehaviour {

    [SerializeField]
    private Transform playerPos;
    private float spring = 7;
    private Transform startPos;
    private Vector3 offset;
    
    // Use this for initialization
	void Start () {
        startPos = playerPos;
        transform.position = new Vector3(playerPos.position.x + transform.position.x, transform.position.y, 0f);
        offset = transform.position - playerPos.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (playerPos != null)
            transform.position = Vector3.Lerp(transform.position, playerPos.position + offset, spring * Time.deltaTime);
        else
            Destroy(gameObject);
    }
}
