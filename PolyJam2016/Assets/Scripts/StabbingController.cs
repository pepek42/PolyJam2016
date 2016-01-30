using UnityEngine;
using System.Collections;

public enum StabbingStage { init = 0, hide = 1, stabbing1 = 2, stabbing2 = 3 };

public class StabbingController : MonoBehaviour {

	public float minigame_speed = 2f;

	public StabbingStage stage = StabbingStage.hide;
	StabbingStage previous_stage = StabbingStage.init;

	int sub_stage;
	float sub_stage_start;

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
			previous_stage = stage;
			break;
			#endregion
		case StabbingStage.stabbing1:
			#region stage.stabbing1
			if (previous_stage != StabbingStage.stabbing1) {
				sub_stage = 0;
				sub_stage_start = Time.time;

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
					stage = StabbingStage.stabbing2;
					current_stabber.GetComponentInChildren<SpriteRenderer> ().enabled = false;
				}
			}
			break;
			#endregion
		case StabbingStage.stabbing2:
			#region stage.stabbing2
			if (previous_stage != StabbingStage.stabbing2) {
				sub_stage = 0;
				sub_stage_start = Time.time;

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
					stage = StabbingStage.hide;
					current_stabber.GetComponentInChildren<SpriteRenderer> ().enabled = false;
				}
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
			if (Mathf.Abs(angle) <= target_treshold) {
				Debug.Log("HIT!");
			} else {
				Debug.Log("Miss!");
			}

		} else if (input <= 0 && action_axis_already_down) {
			action_axis_already_down = false;
		}

		// TODO: DELETE ME!
		if (Input.GetKeyUp(KeyCode.P)) {
			this.stage = StabbingStage.stabbing1;
		}
		#endregion
	}

	public bool Start(WalkerController victim) {
		if (this.stage == StabbingStage.hide) {
			this.victim = victim;
			this.stage = StabbingStage.stabbing1;
			return true;
		} else {
			return false;
		}
	}

	Quaternion MinigameRotation (float rotation) {
		return Quaternion.Euler (new Vector3 (0f, 0f, rotation));
	}
}
