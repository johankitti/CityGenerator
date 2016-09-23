using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CityGenerator : MonoBehaviour {

    public enum District {
        Business, Commercial, Residential, Industrial, Road
    }

    public GameObject TilePrefab;
    public Agent AgentPrefab;

    // Buildings
    GameObject[] BusinessBuildings;
    GameObject[] CommercialBuildings;
    GameObject[] ResidentialBuildings;
    GameObject[] IndustrialBuildings;

    // Roads
    GameObject[] Roads;
    GameObject[] TurnRoads;
    GameObject[] TCrossRoads;
    GameObject[] XCrossRoads;

    // UI
    public Slider NoiseDetailSlider;
    public Slider RandomSeedSlider;
    public Slider BusinessCommercialSlider;
    public Slider ResidentialSlider;
    public Slider IndustrialSlider;
    public Slider SpreadCommercialSlider;
    public Slider VerticalRoadsSlider;
    public Slider HorizontalRoadsSlider;

    const int TileSize = 30;
    const int TilePadding = 5;

    int xPos = -9999;
    int yPos = -9999;

    int CitySize = 100;

	GameObject[,] CityTileMap;
    District[,] CityDistrictMap;
    float[,] CityHeightMap;

    float NoiseDetailX = 10;
    float NoiseDetailY = 10;

    void Start() {
        GenerateCity();

        BusinessBuildings = Resources.LoadAll<GameObject>("Buildings/Business");
        CommercialBuildings = Resources.LoadAll<GameObject>("Buildings/Commercial");
        ResidentialBuildings = Resources.LoadAll<GameObject>("Buildings/Residential");
        IndustrialBuildings = Resources.LoadAll<GameObject>("Buildings/Industrial");

        Roads = Resources.LoadAll<GameObject>("Road/Roads");
        TurnRoads = Resources.LoadAll<GameObject>("Road/TurnRoads");
        TCrossRoads = Resources.LoadAll<GameObject>("Road/TCrossRoads");
        XCrossRoads = Resources.LoadAll<GameObject>("Road/XCrossRoads");

        Debug.Log(Roads.Length);

        NoiseDetailSlider.value = 0.4f;
        RandomSeedSlider.value = 0.0f;
        BusinessCommercialSlider.value = 0.4f;
        ResidentialSlider.value = 0.9f;
        IndustrialSlider.value = 0.6f;
        SpreadCommercialSlider.value = 0.25f;
        VerticalRoadsSlider.value = 0.05f;
        HorizontalRoadsSlider.value = 0.07f;
    }

    void GenerateCity() {
        CityTileMap = new GameObject[CitySize, CitySize];
        CityDistrictMap = new District[CitySize, CitySize];
        CityHeightMap = new float[CitySize, CitySize];

        for (int x = 0; x < CitySize; x++) {
            for (int y = 0; y < CitySize; y++) {
                float instPosX = x * TileSize;
                float instPosY = y * TileSize;

                Vector3 basicTilePos = new Vector3(instPosX, 0, instPosY);

                CityTileMap[x, y] = Instantiate(TilePrefab, Vector3.zero + basicTilePos, Quaternion.identity) as GameObject;
                CityTileMap[x, y].name = "Tile[" + x + ", " + y + "]";
            }
        }
    }


    public void UpdateCity() {
        float noiseLow = 99999;
        float noiseHigh = 0;
        for (int x = 0; x < CitySize; x++) {
            for (int y = 0; y < CitySize; y++) {

                float randomSeed = 100;
                float MainNoise = Mathf.PerlinNoise(
                    (float)x / NoiseDetailX * NoiseDetailSlider.value + randomSeed * RandomSeedSlider.value, 
                    (float)y / NoiseDetailY * NoiseDetailSlider.value + randomSeed * RandomSeedSlider.value);

                float SpreadCommercialNoise = Mathf.PerlinNoise(
                    (float)x / 5,
                    (float)y / 5);

                if (MainNoise < noiseLow)
                    noiseLow = MainNoise;

                if (MainNoise > noiseHigh)
                    noiseHigh = MainNoise;

                float businessValue = 1 - BusinessCommercialSlider.value;
                float commercialValue = businessValue - (1 - ResidentialSlider.value);
                float residentialValue = commercialValue - (1 - IndustrialSlider.value);
                float spreadCommercialValue = 1 - SpreadCommercialSlider.value;

                // Coloring
                if (MainNoise > businessValue) {
                    SetDistrict(District.Business, Color.blue, x, y);
                }
                else if (MainNoise > commercialValue) {
                    SetDistrict(District.Commercial, Color.green, x, y);
                }
                else if (MainNoise > residentialValue) {
                    if (SpreadCommercialNoise > spreadCommercialValue)
                        SetDistrict(District.Commercial, Color.green, x, y);
                    else
                        SetDistrict(District.Residential, Color.red, x, y);
                } else {
                    SetDistrict(District.Industrial, Color.white, x, y);
                }

                CityHeightMap[x, y] = MainNoise;
            }
        }

        GenerateVerticalRoads();
        GenerateHorizontalRoads();
        //Debug.Log(noiseLow + " -> " + noiseHigh);
    }

    void GenerateVerticalRoads() {
        int nrOfRoads = (int)(VerticalRoadsSlider.value * (float)CitySize / 2.0f);
        for (int i = 0; i < nrOfRoads; i++) {
            int roadXPos = Random.Range(0, CitySize);
            int roadYPos = 0;
            int lastStep = -1;

            do {
                SetDistrict(District.Road, Color.black, roadXPos, roadYPos);
                int randomStep = Random.Range(0, 3);
                switch (randomStep) {
                    case (0):
                        if (lastStep != 1) { 
                            roadXPos++;
                            lastStep = randomStep;
                        }
                        if (roadXPos > CitySize - 1)
                            roadXPos = CitySize - 1;
                        break;
   
                    case (1):
                        if (lastStep != 0) {
                            roadXPos--;
                            lastStep = randomStep;
                        }
                        if (roadXPos < 0)
                            roadXPos = 0;
                        break;
                    default:
                        roadYPos++;
                        SetDistrict(District.Road, Color.black, roadXPos, roadYPos);
                        roadYPos++;
                        lastStep = randomStep;
                        break;
                }

            } while (roadYPos < CitySize);
        }
    }

    void GenerateHorizontalRoads() {
        int nrOfRoads = (int)(HorizontalRoadsSlider.value * (float)CitySize / 2.0f);
        for (int i = 0; i < nrOfRoads; i++) {
            int roadXPos = 0;
            int roadYPos = Random.Range(0, CitySize);
            int lastStep = -1;

            do {
                SetDistrict(District.Road, Color.black, roadXPos, roadYPos);
                int randomStep = Random.Range(0, 3);
                switch (randomStep) {
                    case (0):
                        if (lastStep != 1) {
                            roadYPos++;
                            lastStep = randomStep;
                        }
                        if (roadYPos > CitySize - 1)
                            roadYPos = CitySize - 1;
                        break;

                    case (1):
                        if (lastStep != 0) {
                            roadYPos--;
                            lastStep = randomStep;
                        }
                        if (roadYPos < 0)
                            roadYPos = 0;
                        break;
                    default:
                        roadXPos++;
                        SetDistrict(District.Road, Color.black, roadXPos, roadYPos);
                        roadXPos++;
                        lastStep = randomStep;
                        break;
                }

            } while (roadXPos < CitySize);
        }
    }

    public void BuildCity() {
        float part = 1.0f / (float)(CitySize * CitySize);
        for (int x = 0; x < CitySize; x++) {
            for (int y = 0; y < CitySize; y++) {
                GameObject prefab;

                District up = CityDistrictMap[x, Mathf.Max(y - 1, 0)];
                District down = CityDistrictMap[x, Mathf.Min(y + 1, CitySize - 1)];
                District left = CityDistrictMap[Mathf.Max(x - 1, 0), y];
                District right = CityDistrictMap[Mathf.Min(x + 1, CitySize - 1), y];

                Destroy(CityTileMap[x, y]);
                if (CityDistrictMap[x, y] == CityGenerator.District.Road) {
                    InstantiateRoad(up, down, left, right, x, y);
                }
                else {
                    switch (CityDistrictMap[x, y]) {
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

                    Quaternion rotation = GetBuildingRotation(up, down, left, right);
                    GameObject building = Instantiate(prefab, new Vector3(x * TileSize, 0 , y * TileSize), rotation) as GameObject;
                    building.transform.SetParent(transform);

                    SetTileColor(Color.white, x, y);
                }
            }
        }

        SpawnAgents();
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

    void InstantiateRoad(CityGenerator.District up, CityGenerator.District down, CityGenerator.District left, CityGenerator.District right, int x, int y) {
        GameObject prefab = Roads[0];
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 90 * 1, 0));

        // X-CROSSING
        if (up == CityGenerator.District.Road && down == CityGenerator.District.Road && left == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.identity;
            prefab = XCrossRoads[Random.Range(0, XCrossRoads.Length)];
        }

        // T-CROSSING
        else if (up == CityGenerator.District.Road && down == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            prefab = TCrossRoads[Random.Range(0, TCrossRoads.Length)];
        }
        else if (up == CityGenerator.District.Road && down == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 2, 0));
            prefab = TCrossRoads[Random.Range(0, TCrossRoads.Length)];
        }
        else if (up == CityGenerator.District.Road && left == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 3, 0));
            prefab = TCrossRoads[Random.Range(0, TCrossRoads.Length)];
        }
        else if (down == CityGenerator.District.Road && left == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 1, 0));
            prefab = TCrossRoads[Random.Range(0, TCrossRoads.Length)];
        }

        // TURN
        else if (up == CityGenerator.District.Road && right == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            prefab = TurnRoads[Random.Range(0, TurnRoads.Length)];
        }
        else if (up == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            prefab = TurnRoads[Random.Range(0, TurnRoads.Length)];
        }
        else if (right == CityGenerator.District.Road && down == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 3, 0));
            prefab = TurnRoads[Random.Range(0, TurnRoads.Length)];
        }
        else if (down == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 6, 0));
            prefab = TurnRoads[Random.Range(0, TurnRoads.Length)];
        }

        // NORMAL ROAD
        else if (right == CityGenerator.District.Road && left == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 90 * 5, 0));
            prefab = Roads[Random.Range(0, Roads.Length)];
        }
        else if (up == CityGenerator.District.Road && down == CityGenerator.District.Road) {
            rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            prefab = Roads[Random.Range(0, Roads.Length)];
        }

        GameObject building = Instantiate(prefab, new Vector3(x * TileSize, 0, y * TileSize), rotation) as GameObject;
        building.transform.SetParent(transform);
    }

    void SetDistrict(District district, Color color, int x, int y) {
        SetTileColor(color, x, y);
        CityDistrictMap[x, y] = district;
    }

    void SetTileColor(Color color, int x, int y) {
        if (CityTileMap[x, y] != null)
            CityTileMap[x, y].GetComponentInChildren<Renderer>().material.color = color;
    }

    void SpawnAgents() {
        int nrOfAgents = 10;
        for (int i = 0; i < nrOfAgents; i++) {
            int randomX = Random.Range(0, CitySize);
            int randomY = Random.Range(0, CitySize);

            // CityTileMap[randomX, randomY].GetPosition(); eller nåt
            Agent agent = Instantiate(AgentPrefab, new Vector3(randomX * CitySize, 2, randomY * CitySize), Quaternion.identity) as Agent;
            randomX = Random.Range(0, CitySize);
            randomY = Random.Range(0, CitySize);
            agent.Initialize(CityDistrictMap, CitySize, TileSize, randomX, randomY);
        }
    }
}
