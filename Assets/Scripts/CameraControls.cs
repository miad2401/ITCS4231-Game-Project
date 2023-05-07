using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{

    [SerializeField] public int cameraSpeed = 10;
    public void rotateleft(){
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void rotateRight(){
        transform.Rotate(Vector3.up, -90, Space.Self);
    }

    public void Update()
    {
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0)
        {
            p *= cameraSpeed;
        }

        p *= Time.deltaTime;
        Vector3 newPosition = transform.position;
        transform.Translate(p);
        newPosition.x = transform.position.x;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}
