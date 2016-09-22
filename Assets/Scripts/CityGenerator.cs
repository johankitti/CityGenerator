using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CityGenerator : MonoBehaviour {

    public enum District {
        Business, Commercial, Residential, Industrial, Road
    }

    public Transform CameraView2DPos;
    public Tile TilePrefab;
    public GameObject BaseConcrete;
	public GameObject[] Buildings;

    public Agent AgentPrefab;

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

	Tile[,] CityTileMap;
    District[,] CityDistrictMap;

    float NoiseDetailX = 10;
    float NoiseDetailY = 10;

    void Start() {
        GenerateCity();

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
        CityTileMap = new Tile[CitySize, CitySize];
        CityDistrictMap = new District[CitySize, CitySize];

        for (int x = 0; x < CitySize; x++) {
            for (int y = 0; y < CitySize; y++) {
                float instPosX = x * TileSize;
                float instPosY = y * TileSize;

                Vector3 basicTilePos = new Vector3(instPosX, 0, instPosY);

                CityTileMap[x, y] = Instantiate(TilePrefab, Vector3.zero + basicTilePos, Quaternion.identity) as Tile;
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
        int nrOfRoads = (int)(VerticalRoadsSlider.value * (float)CitySize / 2.0f);
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
                CityTileMap[x, y].Build(CityDistrictMap[x, y], 0, CityDistrictMap[x, Mathf.Max(y - 1, 0)], CityDistrictMap[x, Mathf.Min(y + 1, CitySize - 1)], CityDistrictMap[Mathf.Max(x - 1, 0), y], CityDistrictMap[Mathf.Min(x + 1, CitySize - 1), y]);
                SetTileColor(Color.white, x, y);
            }
        }

        SpawnAgents();
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

    /*
        void Update() {
            int newXPos = (int)(CameraView2DPos.position.x / (float)TileSize + 0.5f);
            int newYPos = (int)(CameraView2DPos.position.z / (float)TileSize + 0.5f);

            // If position changed, generate new tiles
            if (newXPos != xPos || newYPos != yPos) {
                Debug.Log("NEW POS: [" + newXPos + ", " + newYPos + "]");

                xPos = newXPos;
                yPos = newYPos;

                UpdateTileArea(xPos - TilePadding, xPos + TilePadding, yPos - TilePadding, yPos + TilePadding);
            }
        }


        void UpdateTileArea(int startTileX, int endTileX, int startTileY, int endTileY) {
            startTileX = (int)Mathf.Max(0, startTileX);
            endTileX = (endTileX > CitySize) ? CitySize : endTileX;

            startTileY = (int)Mathf.Max(0, startTileY);
            endTileY = (endTileY > CitySize) ? CitySize : endTileY;

            Debug.Log("X: " + startTileX + " -> " + endTileX);
            Debug.Log("Y: " + startTileY + " -> " + endTileY);

            for (int x = startTileX; x < endTileX + 1; x++) {
                for (int y = startTileY; y < endTileY + 1; y++) {
                    if (x == startTileX || x == endTileX || y == startTileY || y == endTileY)
                        ChangeTile(false, x, y);
                    else
                        ChangeTile(true, x, y);
                }
            }
        }

        void ChangeTile(bool spawn, int x, int y) {
            if (spawn) {
                if (!CityTileMap[x, y].isActiveAndEnabled) {
                    CityTileMap[x, y].gameObject.SetActive(true);
                    CityTileMap[x, y].Activate(50, 100);
                }
            } else {
                CityTileMap[x, y].gameObject.SetActive(false);
            }
        }
        */
}
