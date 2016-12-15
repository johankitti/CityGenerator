using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public Transform target;

	void Update(){
		transform.LookAt (target.transform);
		transform.RotateAround (target.transform.position, Vector3.up, 10.0f * Time.deltaTime);
	}
}
