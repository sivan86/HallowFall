using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointPowerup : MonoBehaviour {

    [SerializeField]
    private GameObject[] powerups;
    private Transform powerupPos;

	void Start () {
        Game game = GameObject.Find("GameManager").GetComponent<Game>();
        int randomEnemy = Random.Range(0, game.nLevelPowerups);
        Instantiate(powerups[randomEnemy], transform.position, Quaternion.identity);

    }
    private void Update()
    {
        //Debug.Log(enemies[0].transform.position.y);
        if (powerups[0].transform.position.y >= 11.0f)
        {
            Debug.Log("Destroying: " + transform.parent);
            Destroy(transform.parent.gameObject);
        }
    }

}
