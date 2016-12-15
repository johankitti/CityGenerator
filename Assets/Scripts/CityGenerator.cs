﻿using UnityEngine;
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

    // UI
    public Slider NoiseDetailSlider;
    public Slider RandomSeedSlider;
    public Slider BusinessCommercialSlider;
    public Slider ResidentialSlider;
    public Slider IndustrialSlider;
    public Slider SpreadCommercialSlider;
	public Slider MaxRoadLengthSlider;

    const int TileSize = 30;
    const int BasicBlockHeight = 10;
    const int BaseConcreteHeight = 3;
    const int TilePadding = 5;

    int xPos = -9999;
    int yPos = -9999;

    public int CitySize = 100;

    RoadGenerator roadGenerator;

	Tile[,] CityTileMap;

    public District[,] CityDistrictMap;

    float NoiseDetailX = 10;
    float NoiseDetailY = 10;

    void Start() {
        roadGenerator = GetComponent<RoadGenerator>();
        GenerateCity();

        NoiseDetailSlider.value = 0.4f;
        RandomSeedSlider.value = 0.0f;
        BusinessCommercialSlider.value = 0.4f;
        ResidentialSlider.value = 0.9f;
        IndustrialSlider.value = 0.6f;
        SpreadCommercialSlider.value = 0.25f;
        MaxRoadLengthSlider.value = 0.05f;
    }

    void GenerateCity() {
        CityTileMap = new Tile[CitySize, CitySize];
        CityDistrictMap = new District[CitySize, CitySize];
        for (int x = 0; x < CitySize; x++) {
            for (int y = 0; y < CitySize; y++) {
                float instPosX = x * TileSize;
                float instPosY = y * TileSize;

                Vector3 basicTilePos = new Vector3(instPosX, -BasicBlockHeight, instPosY);

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
		roadGenerator.Init((int)MaxRoadLengthSlider.value);
    }

    public void BuildCity() {
        float part = 1.0f / (float)(CitySize * CitySize);
        for (int x = 0; x < CitySize; x++) {
            for (int y = 0; y < CitySize; y++) {
                CityTileMap[x, y].Build(CityDistrictMap[x, y], 0, CityDistrictMap[x, Mathf.Max(y - 1, 0)], CityDistrictMap[x, Mathf.Min(y + 1, CitySize - 1)], CityDistrictMap[Mathf.Max(x - 1, 0), y], CityDistrictMap[Mathf.Min(x + 1, CitySize - 1), y]);
                SetTileColor(Color.white, x, y);
            }
        }
    }

    public void SetDistrict(District district, Color color, int x, int y) {
        SetTileColor(color, x, y);
        CityDistrictMap[x, y] = district;
    }

    void SetTileColor(Color color, int x, int y) {
        if (CityTileMap[x, y] != null)
            CityTileMap[x, y].GetComponentInChildren<Renderer>().material.color = color;
    }
}
