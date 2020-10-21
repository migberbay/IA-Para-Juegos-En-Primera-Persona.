using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Wander : MonoBehaviour{
    public NavMeshAgent wanderer;
    public Transform TopRightMark, BottomLeftMark;
    public GameObject player;
    public PlayerDetection detection;
    public Vector3 LastKnownLocation;
    public bool caughtPlayer = false, following = false;
    public Text stateText, caughtText;
    private int caught_times = 0;

    private void Start() {
        StartCoroutine("SelectNewPosAndMove");
    }

    IEnumerator SelectNewPosAndMove(){ // This is the "Wander" state in the FSM
        // Select the point to raycast from.
        stateText.text = "Wandering...";
        following = false;

        float zCoord = Random.Range(TopRightMark.position.z,BottomLeftMark.position.z);
        float xCoord = Random.Range(TopRightMark.position.x,BottomLeftMark.position.x);
        float yCoord = TopRightMark.position.y;

        // Perform the Raycast straight down.
        RaycastHit hit;
        Physics.Raycast(new Vector3(xCoord,yCoord,zCoord),TopRightMark.transform.up*-1, out hit, 100f);

        // Assign the new destination to agent.
        wanderer.destination = hit.point;

        //we select a new Point every 4 seconds to move the wanderer to.
        yield return new WaitForSeconds(5);
        StartCoroutine("SelectNewPosAndMove");
    }

    IEnumerator FollowPlayer(){ // This is the "Persecución" state in the FSM
        stateText.text = "Chasing...";
        following = true;
        if(detection.visible){
            LastKnownLocation = player.transform.position;
        }
            float d_player = Vector3.Distance(this.gameObject.transform.position, player.gameObject.transform.position);
            float d_location = Vector3.Distance(this.gameObject.transform.position, LastKnownLocation);
            
        if(d_player <= 4 && !caughtPlayer){ // if i caught the player.
            this.StopAllCoroutines();
            caughtPlayer = true;
            caught_times ++;
            caughtText.text = "Caught: " + caught_times;
            StartCoroutine("GetAway");
        }else{ 
            // Set the destination to the last known location
            wanderer.destination = LastKnownLocation;
            if(!caughtPlayer){
                yield return new WaitForSeconds(.25f);
                if(PositionsCloseBy(gameObject.transform.position,LastKnownLocation)){ // si no hemos cazado al jugador pero hemos llegado a su ultima posicion conocida
                    StartCoroutine("SelectNewPosAndMove"); // wander
                }else{ // Si aun no hemos llegado a esa ultima posicion conocida
                    StartCoroutine("FollowPlayer"); // continuamos siguiendole.
                }
            }else{ // this will only happen if the GetAway coroutine has ended.
                caughtPlayer = false;
                if(PositionsCloseBy(gameObject.transform.position,LastKnownLocation)){// if already back to last known position
                    if(detection.visible){ // if we can see it.
                        StartCoroutine("FollowPlayer"); // back to following.
                    }else{  // if not
                        StartCoroutine("SelectNewPosAndMove"); // back to wandering.
                    }
                }else{ // wait 2 seconds and check again.
                    yield return new WaitForSeconds(1);
                    StartCoroutine("FollowPlayer");
                }     
            }
        }
    }

    IEnumerator GetAway(){ // This is the "Alejamiento" state in the FSM
        stateText.text = "Retreating...";

        Vector3 pos_to_run_to = (gameObject.transform.forward * -5) + gameObject.transform.position;
        wanderer.destination = pos_to_run_to;
        // we give it 2 second to run away then go back to following.
        yield return new WaitForSeconds(2);
        StartCoroutine("FollowPlayer");
    }

    private bool PositionsCloseBy(Vector3 a, Vector3 b){
        return Mathf.Abs(a.x - b.x) < 0.1 && Mathf.Abs(a.z - b.z) < 0.1;
    }
}
