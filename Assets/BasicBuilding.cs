using UnityEngine;
using System.Collections;

public class BasicBuilding : MonoBehaviour {

	BoxCollider collider;

	int FramesToSettle = 10;

	private bool Settled = false;

	void Start() {
		collider = GetComponent<BoxCollider> ();
		GetComponent<Renderer> ().enabled = false;
	}

	void Update() {
		FramesToSettle++;
		if (FramesToSettle > 20) {
			Settle();	
		}
	}

	void Settle() {
		collider.isTrigger = false;
		GetComponent<Renderer> ().enabled = true;
		Settled = true;
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log (other.name);
		if (!Settled) {
			Destroy (gameObject);
		}
	}
}
