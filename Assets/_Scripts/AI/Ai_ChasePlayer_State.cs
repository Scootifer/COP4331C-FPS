using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai_ChasePlayer_State : AiState
{
    
    float timer = 0.0f;

    public AiStateId GetId()
    {
        return AiStateId.Chaseplayer;
    }

    public void Enter(AiAgent agent)
    {
        //Debug.Log("CHASE");
        agent.navMeshAgent.stoppingDistance = 5.0f;
    }

    public void Update(AiAgent agent)
    {
        
        if(!agent.enabled)
        {
            return;
        }
        
        timer -= Time.deltaTime;

        if(!agent.navMeshAgent.hasPath)
        {
            agent.navMeshAgent.destination = agent.playerTransform.position;
        }

        Debug.DrawRay(agent._shootPoint.transform.position, agent._shootPoint.transform.forward);
        Vector3 AttackDirection = agent.playerTransform.position - agent.transform.position;
    
        //Debug.Log("ATTACKDIRECTION value" + AttackDirection.magnitude);
        //Debug.Log("STOPPINGdis" + agent.navMeshAgent.stoppingDistance);

        if(AttackDirection.magnitude <= agent.navMeshAgent.stoppingDistance)
        {
            //Debug.Log("DID I STOP?");
            agent.stateMachine.ChangeState(AiStateId.Shoot);
        }

        if(timer < 0.0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.transform.position);
            direction.y = 0;
            
            if(direction.sqrMagnitude < agent.config.maxDistance * agent.config.maxDistance)
            {
                agent.navMeshAgent.destination = agent.playerTransform.position;

            }

            timer = agent.config.maxtime;
        }
    }

    public void Exit(AiAgent agent)
    {
        
    }

    
}
