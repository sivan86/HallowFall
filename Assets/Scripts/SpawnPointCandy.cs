using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointCandy : MonoBehaviour {

    [SerializeField]
    private GameObject[] candies;
    private Transform candyPos;

	void Start () {
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        int randomEnemy = Random.Range(0, game.nLevelCandies);
        Instantiate(candies[randomEnemy], transform.position, Quaternion.identity);

    }
    private void Update()
    {
        //Debug.Log(enemies[0].transform.position.y);
        if (candies[0].transform.position.y >= 11.0f)
        {
            Debug.Log("Destroying: " + transform.parent);
            Destroy(transform.parent.gameObject);
        }
    }

}
