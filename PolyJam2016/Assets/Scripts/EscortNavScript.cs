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
    [SerializeField]
    Vector3 initPosition = new Vector3(6.5f, 2, 7.5f);
    [SerializeField]
    private GameObject capturedDude;

    // Use this for initialization
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Vector3 initPositionWithShift = new Vector3(
            isLeft ? -initPosition.x : initPosition.x,
            initPosition.y,
            initPosition.z
            );
        transform.position = initPositionWithShift;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(capturedDude);//michal ppk
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
    }

    public void setCapturedDude(GameObject _capturedDude)
    {
        Debug.Log(_capturedDude);//michal ppk
        capturedDude = _capturedDude;
    }
}
