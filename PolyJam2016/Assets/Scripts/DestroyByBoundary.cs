using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

    private GameController gameController;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
     }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Dude")
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
