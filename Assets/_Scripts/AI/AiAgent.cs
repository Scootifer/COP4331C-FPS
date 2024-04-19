using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    public SkinnedMeshRenderer mesh;
    public Transform playerTransform;

    public GameObject _eye1;
    public GameObject _eye2;
    public GameObject _shootPoint;
    public GameObject _shootEffectPrefab;
    public GameObject _shootSoundEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {

        if (!Physics.Raycast(transform.position, Vector3.down, 5)) Destroy(gameObject);

        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //shootPoint = GameObject.FindGameObjectWithTag("Attackpoint").transform;
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new Ai_ChasePlayer_State());
        stateMachine.RegisterState(new Ai_Death_State());
        stateMachine.RegisterState(new Ai_Shoot_State());
        stateMachine.RegisterState(new Ai_Idle_State());
        stateMachine.ChangeState(initialState);

        
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
}
