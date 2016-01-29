using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float speed = 15;
    public bool isPlayerOne = true;

    private string horizontal;
    private string vertical;
    private string action;

    // Use this for initialization
    void Start () {
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
        Vector3 newPosition = transform.position;
        newPosition += new Vector3(Input.GetAxis(horizontal) * speed * Time.deltaTime, 0, Input.GetAxis(vertical) * speed * Time.deltaTime);

        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
