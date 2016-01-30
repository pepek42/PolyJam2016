using UnityEngine;
using System.Collections;

public class EscortNavScript : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameController gameController;
    private Vector3 startingPosition;
    [SerializeField]
    private bool isLeft;
    [SerializeField]
    private float shiftX = 3;

    // Use this for initialization
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject capturedDude = gameController.GetCapturedDude();

        if (capturedDude != null)
        {
            Vector3 shift;
            if (isLeft)
            {
                shift = new Vector3(-shiftX, 0, 0);
            }
            else
            {
                shift = new Vector3(shiftX, 0, 0);
            }
            navMeshAgent.SetDestination(capturedDude.transform.position + shift);
        }
        else
        {
            navMeshAgent.SetDestination(startingPosition);
        }
    }
}
