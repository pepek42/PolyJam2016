using UnityEngine;
using System.Collections;

public enum StabbingStage { init = 0, hide = 1, stabbing1 = 2, stabbing2 = 3 };

public class StabbingController : MonoBehaviour {

	public float minigame_speed = 2f;

	SzamanController szaman;

	AudioSource audio_source;
	AudioClip sfx_miss, sfx_fail, sfx_win;

	public StabbingStage stage = StabbingStage.hide;
	StabbingStage previous_stage = StabbingStage.init;

	int sub_stage;
	float sub_stage_start;
	bool skip_stage, fail_minigame;

	GameObject stabber1;
	GameObject stabber2;
	GameObject target;

	GameObject current_stabber;
	Quaternion from_rotation, to_rotation;
	float target_treshold;

	float stabber1_rotation_min = 45f;
	float stabber1_rotation_max = -45f;

	float stabber2_rotation_min = -45f;
	float stabber2_rotation_max = 45f;

	float target_rotation_1 = 0f;
	float target_treshold_1 = 10f;

	float target_rotation_2 = 20f;
	float target_treshold_2 = 10f;

	WalkerController victim;

	string action_axis = "Shaman Action";
	bool action_axis_already_down = false;

	// Use this for initialization
	void Start () {
		stabber1 = GameObject.Find ("Stabber1");
		stabber2 = GameObject.Find ("Stabber2");
		target = GameObject.Find ("Minigame Target");

		audio_source = GetComponentInChildren<AudioSource> ();
		sfx_miss = Resources.Load<AudioClip> ("miss");
		sfx_fail = Resources.Load<AudioClip> ("fail");
		sfx_win = Resources.Load<AudioClip> ("win");

		szaman = GameObject.Find ("Szaman").GetComponent<SzamanController> ();
	}
	
	// Update is called once per frame
	void Update () {
		#region Stages
		switch (stage) {
		case StabbingStage.hide:
			#region stage.hide
			if (previous_stage != StabbingStage.hide) {
				target.GetComponentInChildren<SpriteRenderer> ().enabled = false;
				stabber1.GetComponentInChildren<SpriteRenderer> ().enabled = false;
				stabber2.GetComponentInChildren<SpriteRenderer> ().enabled = false;
			}
			if (previous_stage == StabbingStage.stabbing2) {
				if (fail_minigame) {
					audio_source.PlayOneShot(sfx_fail);
				} else {
					audio_source.PlayOneShot(sfx_win);
				}

				szaman.Stab();

				this.victim.KillDudeAndEscort();
				this.victim = null;
			}
			previous_stage = stage;
			break;
			#endregion
		case StabbingStage.stabbing1:
			#region stage.stabbing1
			if (previous_stage != StabbingStage.stabbing1) {
				sub_stage = 0;
				sub_stage_start = Time.time;
				skip_stage = false;
				fail_minigame = false;

				target.transform.rotation = MinigameRotation(target_rotation_1);
				target.GetComponentInChildren<SpriteRenderer> ().enabled = true;
				target_treshold = target_treshold_1;

				current_stabber = stabber1;
				current_stabber.transform.rotation = MinigameRotation(stabber1_rotation_min);
				current_stabber.GetComponentInChildren<SpriteRenderer> ().enabled = true;

				from_rotation = current_stabber.transform.rotation;
				to_rotation = MinigameRotation(stabber1_rotation_max);
			}
			previous_stage = stage;

			if (!skip_stage) {
				if (sub_stage == 0) {
					current_stabber.transform.rotation = Quaternion.Slerp (from_rotation, to_rotation, (Time.time - sub_stage_start) * minigame_speed);
					float angle = Quaternion.Angle (current_stabber.transform.rotation, to_rotation);
					if (angle < 0.05f) {
						current_stabber.transform.rotation = to_rotation;
						from_rotation = current_stabber.transform.rotation;
						to_rotation = MinigameRotation(stabber1_rotation_min);
						sub_stage = 1;
						sub_stage_start = Time.time;
					}
				} else if (sub_stage == 1) {
					current_stabber.transform.rotation = Quaternion.Slerp (from_rotation, to_rotation, (Time.time - sub_stage_start) * minigame_speed);
					float angle = Quaternion.Angle (current_stabber.transform.rotation, to_rotation);
					if (angle < 0.05f) {
						fail_minigame = true;
						stage = StabbingStage.stabbing2;
						current_stabber.GetComponentInChildren<SpriteRenderer> ().enabled = false;
					}
				}
			} else {
				stage = StabbingStage.stabbing2;
				current_stabber.GetComponentInChildren<SpriteRenderer> ().enabled = false;
			}
			break;
			#endregion
		case StabbingStage.stabbing2:
			#region stage.stabbing2
			if (previous_stage != StabbingStage.stabbing2) {
				sub_stage = 0;
				sub_stage_start = Time.time;
				skip_stage = false;

				target.transform.rotation = MinigameRotation(target_rotation_2);
				target.GetComponentInChildren<SpriteRenderer> ().enabled = true;
				target_treshold = target_treshold_2;

				current_stabber = stabber2;
				current_stabber.transform.rotation = MinigameRotation(stabber2_rotation_min);
				current_stabber.GetComponentInChildren<SpriteRenderer> ().enabled = true;

				from_rotation = current_stabber.transform.rotation;
				to_rotation = MinigameRotation(stabber2_rotation_max);
			}
			previous_stage = stage;

			if (!skip_stage) {
				if (sub_stage == 0) {
					current_stabber.transform.rotation = Quaternion.Slerp (from_rotation, to_rotation, (Time.time - sub_stage_start) * minigame_speed);
					float angle = Quaternion.Angle (current_stabber.transform.rotation, to_rotation);
					if (angle < 0.05f) {
						current_stabber.transform.rotation = to_rotation;
						from_rotation = current_stabber.transform.rotation;
						to_rotation = MinigameRotation(stabber2_rotation_min);
						sub_stage = 1;
						sub_stage_start = Time.time;
					}
				} else if (sub_stage == 1) {
					current_stabber.transform.rotation = Quaternion.Slerp (from_rotation, to_rotation, (Time.time - sub_stage_start) * minigame_speed);
					float angle = Quaternion.Angle (current_stabber.transform.rotation, to_rotation);
					if (angle < 0.05f) {
						fail_minigame = true;
						stage = StabbingStage.hide;
						current_stabber.GetComponentInChildren<SpriteRenderer> ().enabled = false;
					}
				}
			} else {
				stage = StabbingStage.hide;
				current_stabber.GetComponentInChildren<SpriteRenderer> ().enabled = false;
			}
			break;
			#endregion
		}
		#endregion

		#region input
		float input = Input.GetAxis (action_axis);
		if (input > 0 && !action_axis_already_down && (int)stage > 1) {
			action_axis_already_down = true;

			float angle = Quaternion.Angle (target.transform.rotation, current_stabber.transform.rotation);
			if (Mathf.Abs(angle) > target_treshold) {
//				audio_source.PlayOneShot(sfx_miss);
				fail_minigame = true;
			}

			skip_stage = true;

		} else if (input <= 0 && action_axis_already_down) {
			action_axis_already_down = false;
		}

		// TODO: DELETE ME!
//		if (Input.GetKeyUp(KeyCode.P)) {
//			this.stage = StabbingStage.stabbing1;
//		}
		#endregion
	}

	public bool Start(WalkerController victim) {
		if (this.stage == StabbingStage.hide) {
			this.victim = victim;
			szaman.victim = victim;
			this.stage = StabbingStage.stabbing1;
			return true;
		} else {
			Debug.Log ("Killing dude, because I have no time for this shit");
			victim.KillDudeAndEscort ();
			return false;
		}
	}

	Quaternion MinigameRotation (float rotation) {
		return Quaternion.Euler (new Vector3 (0f, 0f, rotation));
	}
}
