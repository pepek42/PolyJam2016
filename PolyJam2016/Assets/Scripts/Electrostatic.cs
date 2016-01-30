using UnityEngine;
using System.Collections;

public class Electrostatic : MonoBehaviour {
    [SerializeField]
    private float magnitudeOfCharge;

    public float getMagOfCharge()
    {
        return magnitudeOfCharge;
    }
}
