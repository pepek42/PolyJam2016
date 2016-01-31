using UnityEngine;
using System.Collections;

public class SzamanController : MonoBehaviour {

	Animator animator;
	public WalkerController victim;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}

	public void Stab () {
		animator.SetTrigger ("Stab");
	}

	public void KillVictim(int stuff) {
		if (victim != null) {
			victim.KillDudeAndEscort ();
			victim = null;
		}
	}
}
