using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public GameObject TileMesh;

    public GameObject[] Roads;
    public GameObject[] RoadTurns;
    public GameObject[] RoadTCrossings;
    public GameObject[] RoadXCrossings;

    public void Build(CityGenerator.District district, GameObject prefab, CityGenerator.District up, CityGenerator.District down, CityGenerator.District left, CityGenerator.District right) {
        Destroy(TileMesh);
        if (district == CityGenerator.District.Road) {
            InstantiateRoad(up, down, left, right);
        } else {
            Quaternion rotation = GetBuildingRotation(up, down, left, right);
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
        GameObject prefab = Roads[0];
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 90 * 1, 0));

        // X-CROSSING
        if (up == CityGenerator.District.Road && down == CityGenerator.District.Road && left == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.identity;
            prefab = RoadXCrossings[Random.Range(0, RoadXCrossings.Length - 1)];
        }

        // T-CROSSING
        else if (up == CityGenerator.District.Road && down == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            prefab = RoadTCrossings[Random.Range(0, RoadTCrossings.Length - 1)];
        }
        else if (up == CityGenerator.District.Road && down == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 2, 0));
            prefab = RoadTCrossings[Random.Range(0, RoadTCrossings.Length - 1)];
        }
        else if (up == CityGenerator.District.Road && left == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 3, 0));
            prefab = RoadTCrossings[Random.Range(0, RoadTCrossings.Length - 1)];
        }
        else if (down == CityGenerator.District.Road && left == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 1, 0));
            prefab = RoadTCrossings[Random.Range(0, RoadTCrossings.Length - 1)];
        }

        // TURN
        else if (up == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            prefab = RoadTurns[Random.Range(0, RoadTurns.Length - 1)];
        }
        else if (up == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            prefab = RoadTurns[Random.Range(0, RoadTurns.Length - 1)];
        }
        else if (right == CityGenerator.District.Road && down == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 3, 0));
            prefab = RoadTurns[Random.Range(0, RoadTurns.Length - 1)];
        }
        else if (down == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 6, 0));
            prefab = RoadTurns[Random.Range(0, RoadTurns.Length - 1)];
        }

        // NORMAL ROAD
        else if (right == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 5, 0));
            prefab = Roads[Random.Range(0, Roads.Length - 1)];
        }
        else if (up == CityGenerator.District.Road && down == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 0, 0));
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
