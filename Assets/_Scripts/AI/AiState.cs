using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateId
{
    Chaseplayer,
    Death,
    Idle,
    Shoot
}

public interface AiState
{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Update(AiAgent agent);
    void Exit(AiAgent agent);
}
