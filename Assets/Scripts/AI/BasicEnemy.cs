using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BasicEnemy : MonoBehaviour
{
    public AI_MANAGER aiManager;
    private NavMeshAgent agent;
    public Vector3 lastSeen;
    public Vector3 idlePos;
    public Transform eyePos;
    RaycastHit hit;
    public LayerMask layer; 
    private float nextActionTime = 0.0f;
    public float period = 3f;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        idlePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        aiManager.visiblityStatus -= Time.deltaTime *2;
        agent.isStopped = Vector3.Distance(transform.position, aiManager.player.transform.position) > 2;
            Vector3 targetDir = aiManager.player.transform.position - eyePos.position;
            float angleToPlayer = (Vector3.Angle(targetDir, eyePos.TransformDirection(Vector3.forward)));
            if(angleToPlayer >= -50 && angleToPlayer <=50) //100FOV
            {
            if(Physics.Raycast(eyePos.position, targetDir, out hit, 20f, layer))
            {
                if(hit.transform.gameObject == aiManager.player)
                    aiManager.visiblityStatus = 100f;
            }
                
            }
            if(aiManager.visiblityStatus > aiManager.combatMargin)
            {
                agent.SetDestination(aiManager.player.transform.position);
                lastSeen = aiManager.player.transform.position;
            }
            if(aiManager.visiblityStatus > aiManager.searchMargin && aiManager.visiblityStatus < aiManager.combatMargin)
            {
                
            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
                agent.SetDestination(lastSeen + new Vector3(Random.insideUnitCircle.x * 5, 0, Random.insideUnitCircle.y * 5));
            }

        }
            if(aiManager.visiblityStatus < aiManager.searchMargin)
            {
                agent.SetDestination(idlePos);
            }
            
       
    }
}
