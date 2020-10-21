
using UnityEngine;
using UnityEngine.AI;


public class PlayerDetection : MonoBehaviour
{   
    public Wander wanderScript;
    public NavMeshAgent wanderer;
    public MeshRenderer m_renderer;
    public Color original, mad;
    public GameObject alert_light;
    public bool visible = false;

    public void CheckIfVisible(GameObject player){
        Vector3 wandererPos = this.gameObject.transform.position;
        wandererPos.y -= 0.2f; 
        Vector3 direction = (player.transform.position - wandererPos).normalized;
        RaycastHit hit;
        
        // Debug.DrawRay(wandererPos,direction * 50, Color.red,2f);
        Physics.Raycast(wandererPos,direction, out hit, 50f); 
        // Debug.Log(hit.collider.name);

        if(hit.collider.gameObject.tag == "Player"){
            Debug.Log("I CAN SEE YOU!!!");
            m_renderer.material.color = mad;
            alert_light.SetActive(true);
            visible = true;
            wanderScript.LastKnownLocation = player.transform.position;
            
            if(!wanderScript.following){
                // We swap to the "Follow" state
                wanderScript.StopAllCoroutines();
                wanderScript.StartCoroutine("FollowPlayer");
            }
        }
    }

    public void OutOfVision(GameObject player){
        Debug.Log("Out of sight, out of mind.");
        m_renderer.material.color = original;
        alert_light.SetActive(false);
        visible = false;
    }
}
