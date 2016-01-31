using UnityEngine;
using System.Collections;

public class SzamanController : MonoBehaviour {

	Animator animator;
	public WalkerController victim;

	AudioSource audio_source;
	AudioClip scream_sfx;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		audio_source = GetComponent<AudioSource> ();
		scream_sfx = Resources.Load<AudioClip> ("scream");
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

	public void Scream () {
		audio_source.PlayOneShot(scream_sfx);
	}
}
