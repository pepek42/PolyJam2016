using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

    public GameController gameController;

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Dude")
        {
            Destroy(other.gameObject.transform.parent.gameObject);
        } else if(other.tag == "Player One" || other.tag == "Player Two")
        {
            gameController.GameOver();
        }
    }
}
