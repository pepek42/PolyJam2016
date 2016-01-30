using UnityEngine;
using System.Collections;

public class WalkerController : MonoBehaviour {
	
	public Transform target;
	public Transform[] players;
	public float newTargetPositionShift = 5;
	public float switchPositionTriggerRadius = 20;

	private Vector3 targetVector;
	private float startSpeed;
	private NavMeshAgent navMeshAgent;

	// Use this for initialization
	void Start () {
		navMeshAgent = GetComponent<NavMeshAgent> ();
		targetVector = new Vector3(1, 0, 0);

		startSpeed = navMeshAgent.speed;
	}
	
	// Update is called once per frame
	void Update () {

		navMeshAgent.speed = startSpeed;

		Vector3 newTargetVector = new Vector3();
		foreach(Transform playerTransform in players)
		{
			Vector3 dude_player_position_diff = transform.position - playerTransform.position;
			dude_player_position_diff.y = 0;
			if (dude_player_position_diff.magnitude < switchPositionTriggerRadius)
			{
				newTargetVector += dude_player_position_diff.normalized / dude_player_position_diff.magnitude;
				if( dude_player_position_diff.magnitude < switchPositionTriggerRadius / 3)
				{
					navMeshAgent.speed += (switchPositionTriggerRadius / 3 - dude_player_position_diff.magnitude) * 4;
				}
			}
			else
			{
				newTargetVector += new Vector3(0.2f, 0, 0);
			}
		}

		targetVector += newTargetVector;

		targetVector.Normalize();
		Vector3 newTargetPosition = transform.position + newTargetPositionShift * targetVector;

		target.position = newTargetPosition;

		navMeshAgent.SetDestination (newTargetPosition);
	}
}
