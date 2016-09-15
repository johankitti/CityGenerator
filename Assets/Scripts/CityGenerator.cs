using UnityEngine;
using System.Collections;

public class CityGenerator : MonoBehaviour {

	public GameObject Building;

	int citySize = 40;
	float buildingSize = 30;
	int[,] city;

	void Start () {
		Debug.Log (Building.transform.localScale);
		city = new int[citySize, citySize];
		Generate ();
	}

	void Generate() {
		for (int x = 0; x < citySize; x++) {
			for (int y = 0; y < citySize; y++) {
				float xPos = x * buildingSize;
				float yPos = y * buildingSize;
				float height = Mathf.PerlinNoise (xPos / 100, yPos / 100) * 120;
				Vector3 instPos = new Vector3 (xPos, height, yPos);
				GameObject instBuilding = Instantiate (Building, Vector3.zero + instPos, Quaternion.identity) as GameObject;
			}
		}
	}
}
