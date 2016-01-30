using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float speed = 15;
    public bool isPlayerOne = true;

	Animator animator;

    private string horizontal;
    private string vertical;
    private string action;

    // Use this for initialization
    void Start () {
		animator = GetComponentInChildren<Animator> ();

        // choosing right player inputs
        if(isPlayerOne)
        {
            horizontal = "Player One Horizontal";
            vertical = "Player One Vertical";
            action = "Player One Action";
        }
        else
        {
            horizontal = "Player Two Horizontal";
            vertical = "Player Two Vertical";
            action = "Player Two Action";
        }
    }

    void FixedUpdate()
    {
        // input normalization
        Vector2 inputs = new Vector2(Input.GetAxis(horizontal), Input.GetAxis(vertical));
        if (inputs.magnitude > 1)
        {
            inputs.Normalize();
        }

		if (inputs.magnitude <= 0) {
			animator.SetBool ("walking", false);
		} else {
			animator.SetBool ("walking", true);
		}

        // update player position
        Vector3 positionSwitch = new Vector3(inputs.x * speed * Time.deltaTime, 0, inputs.y * speed * Time.deltaTime);
        transform.position += positionSwitch;
            
        // we don't want player to fall
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
