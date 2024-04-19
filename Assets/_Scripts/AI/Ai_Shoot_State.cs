using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai_Shoot_State : AiState
{
    float timer;

    

    public AiStateId GetId()
    {
        return AiStateId.Shoot;
    }

    public void Enter(AiAgent agent)
    {
        timer = 2.0f;
        agent.navMeshAgent.stoppingDistance = 3.0f;
        
        //fire effects
        GameObject effect1 = GameObject.Instantiate(agent._shootEffectPrefab, Vector3.zero, agent._eye1.transform.rotation);
        GameObject effect2 = GameObject.Instantiate(agent._shootEffectPrefab, Vector3.zero, agent._eye2.transform.rotation);

        //sound effect
        GameObject effect3 = GameObject.Instantiate(agent._shootSoundEffectPrefab, Vector3.zero, agent._eye2.transform.rotation);

        effect1.transform.SetParent(agent.transform);
        effect2.transform.SetParent(agent.transform);

        effect1.transform.localPosition = new Vector3(0.1f, 1f, 0.1f);
        effect2.transform.localPosition = new Vector3(-0.1f, 1f, -0.1f);

        ShootPointCollider collider = agent._shootPoint.GetComponent<ShootPointCollider>();
        List<GameObject> collisionList = collider._collisionList;

        EntityHealthComponent EHC;

        foreach(GameObject obj in collisionList)
        {
            if (obj.TryGetComponent<EntityHealthComponent>(out EHC))
            {
                Debug.Log($"Damaging {obj.transform.name}");
                EHC.ApplyDamage(10);
            }
        }
        
    }

    public void Update(AiAgent agent)
    {
        agent.navMeshAgent.destination = agent.playerTransform.position;

        timer -= Time.deltaTime;

        if(timer < 0.0f)
        {
            Vector3 AttackDirection = agent.playerTransform.position - agent.transform.position;

            if(AttackDirection.magnitude >= agent.navMeshAgent.stoppingDistance)
            {
                agent.stateMachine.ChangeState(AiStateId.Chaseplayer);
            }
            else
            {
                agent.stateMachine.ChangeState(AiStateId.Shoot);
            }
        }
    }


    public void Exit(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }
}
