using UnityEngine;
using System.Collections;

public class CityGenerator : MonoBehaviour {

	public GameObject[] Buildings;

	int citySize = 30;
	float buildingSize = 30;
	int[,] city;

	void Start () {
		city = new int[citySize, citySize];
		Generate ();
	}

	void Generate() {
		for (int x = 0; x < citySize; x++) {
			for (int y = 0; y < citySize; y++) {
				float xPos = x * buildingSize;
				float yPos = y * buildingSize;
				float height = Mathf.PerlinNoise (xPos / 100, yPos / 100) * 120;
				Vector3 instPos = new Vector3 (xPos, 0, yPos);	
				//Debug.Log (height);

				GameObject toInst;

				if (height < 20) {
					toInst = Buildings [0];
				} else if (height < 40) {
					toInst = Buildings [1];
				} else if (height < 60) {
					toInst = Buildings [2];
				} else if (height < 70) {
					toInst = Buildings [3];
				} else if (height < 80) {
					toInst = Buildings [4];
				} else {
					toInst = Buildings [5];
				}
				// Buildings[Random.Range(0, Buildings.Length)]
				GameObject instBuilding = Instantiate (toInst, Vector3.zero + instPos, Quaternion.identity) as GameObject;
			}
		}
	}
}
