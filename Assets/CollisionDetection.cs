using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        // Debug.Log("Hello im triggered!, THAT DUDE DID IT: "+ other.gameObject.name);
        if(other.gameObject.tag == "Player"){
            gameObject.GetComponentInParent<PlayerDetection>().CheckIfVisible(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player"){
            gameObject.GetComponentInParent<PlayerDetection>().OutOfVision(other.gameObject);
        }
    }
}
