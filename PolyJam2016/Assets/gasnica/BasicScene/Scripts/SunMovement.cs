using UnityEngine;
using System.Collections;

// This simply rotates the direcitonal light to simulate sun. 
// And it adjust the color of the light also
public class SunMovement : MonoBehaviour {

  public Light Light;

  // Game length in seconds. Sun's movement is syncronized with it.
  public float GameLength = 20;
  private float PlayTime;
  
	// Use this for initialization
	void Start () {
     PlayTime = 0;
     Quaternion tmp = Quaternion.identity;
     //tmp.SetFromToRotation(new Vector3(-2, 0, 1), Vector3.zero);
     tmp.SetLookRotation(new Vector3(2, 0, -1));
     Light.transform.rotation = tmp;
     //Light.transform.rotation.SetLookRotation(new Vector3(2, 0, -1));
	}
  	
	// Update is called once per frame
	void Update () {

    
    // Update intensity
    float maxIntensity = 1;
    float minIntensity = 0;
    Light.intensity = Mathf.Abs(Mathf.Sin(PlayTime / GameLength * Mathf.PI) * (maxIntensity - minIntensity)) + minIntensity;
    
    // Rotate across sky
    Quaternion deltaRotation = Quaternion.AngleAxis(-180 * Time.deltaTime / GameLength, Matrix4x4.identity.GetColumn(2));
    Light.transform.rotation = deltaRotation * Light.transform.rotation;
    
    PlayTime += Time.deltaTime;
	}
}
