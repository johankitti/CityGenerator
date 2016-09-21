using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public GameObject TileMesh;

    public GameObject Road;
    public GameObject RoadTurn;
    public GameObject RoadTCrossing;
    public GameObject RoadXCrossing;

    public GameObject[] BusinessBuildings;
    public GameObject[] CommercialBuildings;
    public GameObject[] ResidentialBuildings;
    public GameObject[] IndustrialBuildings;

    public void Build(CityGenerator.District district, float height, CityGenerator.District up, CityGenerator.District down, CityGenerator.District left, CityGenerator.District right) {
        Destroy(TileMesh);
        if (district == CityGenerator.District.Road) {
            InstantiateRoad(up, down, left, right);
        } else {
            Quaternion rotation = GetBuildingRotation(up, down, left, right);
            GameObject prefab;

            switch (district) {
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
                default:
                    prefab = ResidentialBuildings[Random.Range(0, ResidentialBuildings.Length - 1)];
                    break;
            }
            GameObject building = Instantiate(prefab, transform.localPosition, rotation) as GameObject;
            building.transform.SetParent(transform);
        }
    }

    Quaternion GetBuildingRotation(CityGenerator.District up, CityGenerator.District down, CityGenerator.District left, CityGenerator.District right) {
        if (up == CityGenerator.District.Road) {
            return Quaternion.Euler(new Vector3(0, 90 * 3, 0));
        }
        else if (right == CityGenerator.District.Road) {
            return Quaternion.Euler(new Vector3(0, 90 * 2, 0));
        }
        else if (down == CityGenerator.District.Road) {
            return Quaternion.Euler(new Vector3(0, 90 * 1, 0));
        }
        else if (left == CityGenerator.District.Road) {
            return Quaternion.Euler(new Vector3(0, 90 * 4, 0));
        }
        else {
            return Quaternion.Euler(new Vector3(0, 90 * Random.Range(0, 3), 0));
        }
    }

    void InstantiateRoad(CityGenerator.District up, CityGenerator.District down, CityGenerator.District left, CityGenerator.District right) {
        GameObject prefab = Road;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 90 * 1, 0));
        string name = "";

        // X-CROSSING
        if (up == CityGenerator.District.Road && down == CityGenerator.District.Road && left == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.identity;
            prefab = RoadXCrossing;
        }

        // T-CROSSING
        else if (up == CityGenerator.District.Road && down == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            prefab = RoadTCrossing;
        }
        else if (up == CityGenerator.District.Road && down == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 2, 0));
            prefab = RoadTCrossing;
        }
        else if (up == CityGenerator.District.Road && left == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 3, 0));
            prefab = RoadTCrossing;
        }
        else if (down == CityGenerator.District.Road && left == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 1, 0));
            prefab = RoadTCrossing;
        }

        // TURN
        else if (up == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            prefab = RoadTurn;
            name = "1";
        }
        else if (up == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            prefab = RoadTurn;
            name = "2";
        }
        else if (right == CityGenerator.District.Road && down == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 3, 0));
            prefab = RoadTurn;
            name = "3";
        }
        else if (down == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 6, 0));
            prefab = RoadTurn;
            name = "4";
        }

        // NORMAL ROAD
        else if (right == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 5, 0));
            prefab = Road;
            name = "1";
        }
        else if (up == CityGenerator.District.Road && down == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            prefab = Road;
            name = "2";
        }

        GameObject building = Instantiate(prefab, transform.localPosition, rotation) as GameObject;
        building.transform.SetParent(transform);
        building.name = name;
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
