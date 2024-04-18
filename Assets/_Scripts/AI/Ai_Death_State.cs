using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Death_State : AiState
{
    public Vector3 direction;
    
    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Enter(AiAgent agent)
    {
        direction.y = 1;
        agent.mesh.updateWhenOffscreen = true;
        //Destroy(agent.GameObject);
        Debug.Log("Death");
    }

    public void Update(AiAgent agent)
    {

    }

    public void Exit(AiAgent agent)
    {
        
    }
}
