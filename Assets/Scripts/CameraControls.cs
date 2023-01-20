using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public void rotateleft(){
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void rotateRight(){
        transform.Rotate(Vector3.up, -90, Space.Self);
    }
}
