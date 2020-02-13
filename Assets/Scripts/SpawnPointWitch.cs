using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointWitch : MonoBehaviour {

    [SerializeField]
    private GameObject[] enemies;
    private float enemyPosX;

	void Start () {
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        int randomEnemy = Random.Range(0, game.nLevelEnemies);
        //enemyPosX = Random.Range(-3.0f, 3.5f);
        Instantiate(enemies[randomEnemy], transform.position, Quaternion.identity);
    }
    private void Update()
    {
        //Debug.Log(enemies[0].transform.position.y);
        if (enemies[0].transform.position.y >= 11.0f)
        {
            Debug.Log("Destroying: " + transform.parent);
            Destroy(transform.parent.gameObject);
        }
    }

}
