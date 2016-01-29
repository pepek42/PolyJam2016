using UnityEngine;
using System.Collections;

public class WalkerController : MonoBehaviour {

	NavMeshAgent navMeshAgent;
	public Transform target;

	// Use this for initialization
	void Start () {
		navMeshAgent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		navMeshAgent.SetDestination (target.position);
	}
}
