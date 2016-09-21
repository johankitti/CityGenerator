using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

    float Speed = 80;

	void Update () {
        Vector3 moveVector = Vector3.zero;
                
        if (Input.GetKey(KeyCode.LeftArrow)) {
            moveVector += new Vector3(-1, 0, 1);
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            moveVector += new Vector3(1, 0, -1);
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            moveVector += new Vector3(1, 0, 1);
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            moveVector += new Vector3(-1, 0, -1);
        }
        transform.position += moveVector * Time.deltaTime * Speed;
    }
}
