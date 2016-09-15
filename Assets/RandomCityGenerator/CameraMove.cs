using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public Transform target;

	void Update(){
		transform.LookAt (Vector3.zero);
		transform.RotateAround (Vector3.zero, Vector3.up, 10.0f * Time.deltaTime);
	}
}
