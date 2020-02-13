using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyEffect : MonoBehaviour {

    [SerializeField]
    Vector3 targetposition;

    // Update is called once per frame
    void Update () {
        //targetposition = new Vector3(-4.65f, 7.55f, 0f);
        transform.position = Vector3.MoveTowards(transform.position, targetposition, 9*Time.deltaTime);
       
    }
}
