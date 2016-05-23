using UnityEngine;
using System.Collections;

public class FieldOfViewController : MonoBehaviour
{
    public CameraController cameraController;

    void OnTriggerEnter2D(Collider2D baseCollider)
    {
        if (baseCollider.gameObject.tag == "Pipe") baseCollider.gameObject.GetComponent<PipeController>().isInFieldOfView = true;
        else if (baseCollider.gameObject.tag == "Finish") cameraController.canMove = false;
    }
}
