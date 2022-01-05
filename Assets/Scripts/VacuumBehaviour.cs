using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VacuumBehaviour : MonoBehaviour
{
    public enum AIState
    {
        Passive,
        Normal,
        SpedUp
    };

    public AIState curState;
    public Transform target;
    public float normalSpeed;
    public float speedUpSpeed;
    public float speedUpDist;
    public float stopDist = 0f;
    private NavMeshAgent agent;

    [Header("Debug")]
    public float debugDist;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stopDist;
    }

    // Update is called once per frame
    void Update()
    {
        if (curState != AIState.Passive)
        {
            agent.destination = target.position;
            if (GetPathRemainingDistance(agent) > speedUpDist) { agent.speed = speedUpSpeed; curState = AIState.SpedUp; }
            else { agent.speed = normalSpeed; curState = AIState.Normal; }
        }

        debugVals();
    }

    public void debugVals()
    {
        debugDist = GetPathRemainingDistance(agent);
    }

    // TY forum user, luckily I found out about this even though it's not documented. agent.remainingDistance won't work as I have corners.
    // https://stackoverflow.com/questions/61421172/why-does-navmeshagent-remainingdistance-return-values-of-infinity-and-then-a-flo
    public float GetPathRemainingDistance(NavMeshAgent navMeshAgent)
    {
        // Returns if the path is not there, path is invalid or the endpoint has been reached.
        if (navMeshAgent.pathPending ||
            navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid ||
            navMeshAgent.path.corners.Length == 0)
            return -1f;

        // Loop through the points and add up the distance.
        float distance = 0.0f;
        for (int i = 0; i < navMeshAgent.path.corners.Length - 1; ++i)
        {
            distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
        }

        return distance;
    }
}
