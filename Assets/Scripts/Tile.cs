using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public GameObject TileMesh;

    public GameObject Road;
    public GameObject[] BusinessBuildings;
    public GameObject[] CommercialBuildings;
    public GameObject[] ResidentialBuildings;
    public GameObject[] IndustrialBuildings;

    public void Build(CityGenerator.District district, float height) {
        Destroy(TileMesh);
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 90 * Random.Range(0, 3), 0));

        GameObject prefab;

        switch(district) {
            case (CityGenerator.District.Business):
                prefab = BusinessBuildings[Random.Range(0, BusinessBuildings.Length)];
                break;
            case (CityGenerator.District.Commercial):
                prefab = CommercialBuildings[Random.Range(0, CommercialBuildings.Length)];
                break;
            case (CityGenerator.District.Residential):
                prefab = ResidentialBuildings[Random.Range(0, ResidentialBuildings.Length)];
                break;
            /*
        case (CityGenerator.District.Industrial):
            prefab = IndustrialBuildings[Random.Range(0, IndustrialBuildings.Length - 1)];
            break;
            */
            case (CityGenerator.District.Road):
                prefab = Road;
                break;
            default:
                prefab = ResidentialBuildings[Random.Range(0, ResidentialBuildings.Length - 1)];
                break;
        }
        GameObject building = Instantiate(prefab, transform.localPosition, rotation) as GameObject;
        building.transform.SetParent(transform);
    }

    /*
    Vector3 StartPos;

    public void SetStartPos() {
        StartPos = transform.localPosition;
    }

    public void Activate(float dropHeight, float dropSpeed) {
        transform.localPosition += Vector3.up * dropHeight;
        StartCoroutine(Drop(dropHeight, dropSpeed));
    }

    IEnumerator Drop(float dropHeight, float dropSpeed) {
        while(transform.localPosition.y > 0) {
            transform.localPosition += Vector3.down * dropSpeed * Time.deltaTime;
            yield return null;
        }

        transform.localPosition = StartPos;
    }
    */
}
