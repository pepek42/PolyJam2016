using UnityEngine;
using System.Collections;

public class DestroyByCapture : MonoBehaviour
{
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dude")
        {
            WalkerController walkerController = other.GetComponentInParent<WalkerController>();
            if (!walkerController.IsCaptured())
            {
                GameObject newTarget = GameObject.FindGameObjectWithTag("Dudes slaughter point");
                if (newTarget != null)
                {
                    walkerController.CaptureAndSetNewTarget(newTarget.transform);
                }
                else
                {
                    Debug.Log("DestroyByCapture::OnTriggerEnter - Should not happened");
                }
            }
        }
    }
}
