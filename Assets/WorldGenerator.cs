using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldGenerator : MonoBehaviour {

	public GameObject BasicBuilding;
	public GameObject BuildingsHolder;

	public Image SliderFill;
	public Image SliderBG;

	private int BuildingIndex = 0;
	private int BuildingCount = 500;

	private float TerrainHalfSize = 500.0f;
	private bool Generating;

	void Start() {
		SliderFill.fillAmount = 0.0f;
		ShowUI (false);
	}

	void Update() {
		if (Generating) {
			if (transform.childCount < BuildingCount) {
				PlaceBuilding ();;
			} else {
				Generating = false;
				ShowUI (false);
			}
		}
	}

	public void GenerateWorld() {
		foreach(Transform child in transform) {
			Destroy(child.gameObject);
		}
		BuildingIndex = 0;
		ShowUI (true);
		Generating = true;
	}

	public void ShowUI(bool show) {
		SliderFill.enabled = show;
		SliderBG.enabled = show;
	}

	void PlaceBuilding() {
		Vector3 position = new Vector3 (
			Random.Range (-TerrainHalfSize, TerrainHalfSize), 
			100, 
			Random.Range (-TerrainHalfSize, TerrainHalfSize));
		float height = Mathf.PerlinNoise (position.x/150.0f, position.z/150.0f) * 200;
		GameObject building = Instantiate (BasicBuilding, position, Quaternion.identity) as GameObject;
		building.transform.localScale = new Vector3 (
			building.transform.localScale.x * Random.Range (0.5f, 1.5f),
			height, 
			building.transform.localScale.z * Random.Range (0.5f, 1.5f));
		building.transform.rotation = Quaternion.Euler (new Vector3 (
			0,
			Random.Range (0, 360),
			0
		));
		building.transform.parent = transform;
	
		building.transform.position = new Vector3 (
			building.transform.position.x,
			height / 2,
			building.transform.position.z
		);

		building.gameObject.name = "Building-" + BuildingIndex;
		BuildingIndex++;
		float newFillValue = (float)transform.childCount / BuildingCount;
		if (newFillValue > SliderFill.fillAmount) {
			SliderFill.fillAmount = newFillValue;
		}
	}
}
