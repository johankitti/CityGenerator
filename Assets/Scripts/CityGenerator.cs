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
				GameObject instBuilding = Instantiate (Buildings[Random.Range(0, Buildings.Length)], Vector3.zero + instPos, Quaternion.identity) as GameObject;
			}
		}
	}
}
