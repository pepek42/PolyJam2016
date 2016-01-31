using UnityEngine;
using System.Collections;

public class EscortController : MonoBehaviour {

	Animator[] animators;

	// Use this for initialization
	void Start () {
		animators = GetComponentsInChildren<Animator> ();
		foreach (Animator animator in animators) {
			animator.SetBool ("walking", true);
		}
	}
}
