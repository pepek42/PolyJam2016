using UnityEngine;
using System.Collections;

public class WalkerController : MonoBehaviour {
    
    public Transform target;
    public Transform[] players;
    public float newTargetPosMultiplier = 5;
    public float switchPosTriggerRadius = 30;
    public float increaseSpeedThreshold = 5;
    public float switchSensivity = 20;
    public float dudeAcceleration = 10;
    public float dudedeceleration = 2;
    public GameObject escort;
    public GameObject escortInstance;

    private float minimumDistance = 2;
    private Vector3 targetVector;
    private float startSpeed;
    private NavMeshAgent navMeshAgent;
    private bool isCaptured;

    // Use this for initialization
    void Start () {
        navMeshAgent = GetComponent<NavMeshAgent> ();
        targetVector = new Vector3(1, 0, 0);

        startSpeed = navMeshAgent.speed;


        players[0] = GameObject.FindGameObjectWithTag("Player One").GetComponent<Transform>();
        players[1] = GameObject.FindGameObjectWithTag("Player Two").GetComponent<Transform>();

        isCaptured = false;
    }
    
    // Update is called once per frame
    void Update () {

        Vector3 newTargetPosition;

        if (!isCaptured)
        {
            //updating dude NavMeshAgent component
            Vector3 newTargetVector = new Vector3();
            //players iteration
            foreach (Transform playerTransform in players)
            {
                bool speedIncreased = false;
                Vector3 dude_player_position_diff = transform.position - playerTransform.position;
                dude_player_position_diff.y = 0;
                //Vector2 dude_player_position_diff_2d = new Vector2(dude_player_position_diff.x, dude_player_position_diff.z);
                //float distance = dude_player_position_diff_2d.magnitude - minimumDistance;
                // player to dude distance
                float distance = Mathf.Sqrt(Mathf.Pow(dude_player_position_diff.x, 2) + Mathf.Pow(dude_player_position_diff.z, 2)) - minimumDistance;
                if (distance < 0.1)
                {
                    distance = 0.1f;
                }

                if (distance < switchPosTriggerRadius)
                {
                    newTargetVector += dude_player_position_diff.normalized / Mathf.Pow(distance, 1);
                    if (distance < switchPosTriggerRadius / 3)
                    {
                        speedIncreased = true;
                        navMeshAgent.speed += dudeAcceleration * Time.deltaTime;
                    }
                }
                else
                {
                    newTargetVector += new Vector3(0.2f, 0, 0);
                }
                if (!speedIncreased && navMeshAgent.speed > startSpeed)
                {
                    navMeshAgent.speed -= dudedeceleration * Time.deltaTime;
                    if (navMeshAgent.speed < startSpeed)
                    {
                        navMeshAgent.speed = startSpeed;
                    }
                }
            }

            targetVector += newTargetVector * Time.deltaTime * switchSensivity;

            targetVector.Normalize();
            newTargetPosition = transform.position + newTargetPosMultiplier * targetVector;

            target.position = newTargetPosition;
        }
        else
        {
            newTargetPosition = target.position;

            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        Destroy(this.gameObject);
                        Destroy(escortInstance);
                    }
                }
            }

        }
        if(navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.SetDestination(newTargetPosition);
        }
    }

    public void CaptureAndSetNewTarget(Transform newTarget)
    {
        target = newTarget;
        isCaptured = true;
        navMeshAgent.speed /= 3;
        escortInstance = Instantiate(escort);
        Debug.Log(escortInstance);//michal ppk
        Transform escorts = escortInstance.transform;
        Debug.Log(escorts);//michal ppk
        foreach(Transform escortDude in escorts)
        {
            Debug.Log(escortDude.tag);
            if (escortDude.tag == "Escort One" || escortDude.tag == "Escort Two")
            {
                EscortNavScript escortScript = escortDude.gameObject.GetComponent<EscortNavScript>();
                Debug.Log(escortScript);//michal ppk
                escortScript.setCapturedDude(gameObject);
            }
        }
    }

    public bool IsCaptured()
    {
        return isCaptured;
    }

}
