using UnityEngine;
using System.Collections;

public enum StabbingStage { init, hide, stabbing1, stabbing2 };

public class StabbingController : MonoBehaviour {

	public float minigame_speed = 2f;

	StabbingStage stage = StabbingStage.hide;
	StabbingStage previous_stage = StabbingStage.init;

	int sub_stage;
	float sub_stage_start;

	GameObject stabber1;
	GameObject stabber2;

	Quaternion starting_rotation, target_rotation;

	WalkerController victim;

	string action_axis = "Shaman Action";

	// Use this for initialization
	void Start () {
		stabber1 = GameObject.Find ("Stabber1");
		stabber2 = GameObject.Find ("Stabber2");
	}
	
	// Update is called once per frame
	void Update () {
		switch (stage) {
		case StabbingStage.hide:
			if (previous_stage != StabbingStage.hide) {
				stabber1.GetComponentInChildren<SpriteRenderer> ().enabled = false;
				stabber2.transform.rotation = Quaternion.AngleAxis (-45, Vector3.forward);
				stabber2.GetComponentInChildren<SpriteRenderer> ().enabled = false;
			}
			previous_stage = stage;
			break;
		case StabbingStage.stabbing1:
			if (previous_stage != StabbingStage.stabbing1) {
				stabber1.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 45f));
				stabber1.GetComponentInChildren<SpriteRenderer> ().enabled = true;
				sub_stage = 0;
				sub_stage_start = Time.time;
				starting_rotation = stabber1.transform.rotation;
				target_rotation = Quaternion.Euler (new Vector3 (0f, 0f, -45f));
			}
			previous_stage = stage;

			if (sub_stage == 0) {
				stabber1.transform.rotation = Quaternion.Slerp (starting_rotation, target_rotation, (Time.time - sub_stage_start) * minigame_speed);
				float angle = Quaternion.Angle (stabber1.transform.rotation, target_rotation);
				if (angle < 0.05f) {
					stabber1.transform.rotation = target_rotation;
					starting_rotation = stabber1.transform.rotation;
					target_rotation = Quaternion.Euler (new Vector3 (0f, 0f, 45f));
					sub_stage = 1;
					sub_stage_start = Time.time;
				}
			} else if (sub_stage == 1) {
				stabber1.transform.rotation = Quaternion.Slerp (starting_rotation, target_rotation, (Time.time - sub_stage_start) * minigame_speed);
				float angle = Quaternion.Angle (stabber1.transform.rotation, target_rotation);
				if (angle < 0.05f) {
					stage = StabbingStage.stabbing2;
					stabber1.GetComponentInChildren<SpriteRenderer> ().enabled = false;
				}
			}
			break;
		case StabbingStage.stabbing2:
			if (previous_stage != StabbingStage.stabbing2) {
				stabber2.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, -45f));
				stabber2.GetComponentInChildren<SpriteRenderer> ().enabled = true;
				sub_stage = 0;
				sub_stage_start = Time.time;
				starting_rotation = stabber2.transform.rotation;
				target_rotation = Quaternion.Euler (new Vector3 (0f, 0f, 45f));
			}
			previous_stage = stage;

			if (sub_stage == 0) {
				stabber2.transform.rotation = Quaternion.Slerp (starting_rotation, target_rotation, (Time.time - sub_stage_start) * minigame_speed);
				float angle = Quaternion.Angle (stabber2.transform.rotation, target_rotation);
				if (angle < 0.05f) {
					stabber2.transform.rotation = target_rotation;
					starting_rotation = stabber2.transform.rotation;
					target_rotation = Quaternion.Euler (new Vector3 (0f, 0f, -45f));
					sub_stage = 1;
					sub_stage_start = Time.time;
				}
			} else if (sub_stage == 1) {
				stabber2.transform.rotation = Quaternion.Slerp (starting_rotation, target_rotation, (Time.time - sub_stage_start) * minigame_speed);
				float angle = Quaternion.Angle (stabber2.transform.rotation, target_rotation);
				if (angle < 0.05f) {
					stage = StabbingStage.hide;
					stabber2.GetComponentInChildren<SpriteRenderer> ().enabled = false;
				}
			}

			break;
		}

		// TODO: DELETE ME!
		if (Input.GetKeyUp(KeyCode.P)) {
			this.stage = StabbingStage.stabbing1;
		}
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
}
