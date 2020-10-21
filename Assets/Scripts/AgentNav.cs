using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentNav : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject posMark;

    private void Update() {
        if (Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(camRay, out hit, 100)){
                agent.destination = hit.point;

                GameObject activePosMarker = GameObject.FindGameObjectWithTag("PosMarker");
                if(activePosMarker != null){
                    Destroy(activePosMarker);
                }
                Quaternion xrot =  Quaternion.Euler(90,0,0);
                Vector3 elevated = hit.point;
                elevated.y += .1f;
                activePosMarker = Instantiate(posMark,elevated,xrot);
                StartCoroutine("SpawnPosMarker",activePosMarker);
            }
        }
    }

    IEnumerator SpawnPosMarker(GameObject posMarkerActive) {
        yield return new WaitForSeconds(3.5f);
        Destroy(posMarkerActive);
    }
}