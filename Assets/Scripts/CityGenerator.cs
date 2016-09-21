using UnityEngine;
using System.Collections;

public class CityGenerator : MonoBehaviour {

    public Transform CameraView2DPos;
    public GameObject BasicBlock;
    public GameObject BaseConcrete;
	public GameObject[] Buildings;

    const int TileSize = 60;
    const int BasicBlockHeight = 10;
    const int BaseConcreteHeight = 3;
    const int TilePadding = 3;

    int xPos = 0;
    int yPos = 0;

    int CitySize = 40;

    int[,] CityTileMap;
	GameObject[,] CityGameObjects;

    
    void Start() {
        GenerateCity();
    }

    void Update() {
        int newXPos = (int)(CameraView2DPos.position.x / (float)TileSize + 0.5f);
        int newYPos = (int)(CameraView2DPos.position.z / (float)TileSize + 0.5f);

        // If position changed, generate new tiles
        if (newXPos != xPos || newYPos != yPos) {
            Debug.Log("NEW POS: [" + newXPos + ", " + newYPos + "]");

            if (newXPos > xPos) {
                ChangeTileArea(true, newXPos + TilePadding, newXPos + TilePadding, newYPos - TilePadding, newYPos + TilePadding);
                ChangeTileArea(false, newXPos - TilePadding, newXPos - TilePadding, newYPos - TilePadding, newYPos + TilePadding);
            } else if (newXPos < xPos) {
                ChangeTileArea(true, newXPos - TilePadding, newXPos - TilePadding, newYPos - TilePadding, newYPos + TilePadding);
                ChangeTileArea(false, newXPos + TilePadding, newXPos + TilePadding, newYPos - TilePadding, newYPos + TilePadding);
            }

            if (newYPos > yPos) {

            }
            else if (newYPos < yPos) {

            }

            SetColor(Color.red, newXPos, newYPos);
            SetColor(Color.white, xPos, yPos);

            xPos = newXPos;
            yPos = newYPos;
        }
    }

    void GenerateCity() {
        CityGameObjects = new GameObject[CitySize, CitySize];

        CityTileMap = new int[CitySize, CitySize];

        for (int x = 0; x < CitySize; x++) {
            for (int y = 0; y < CitySize; y++) {
                CityTileMap[x, y] = 1;
            }
        }

        ChangeTileArea(true, xPos - TilePadding, xPos + TilePadding, yPos - TilePadding, yPos + TilePadding);
    }

    void ChangeTileArea(bool spawn, int startTileX, int endTileX, int startTileY, int endTileY) {
        startTileX = (int)Mathf.Max(0, startTileX);
        endTileX = (endTileX > CitySize) ? CitySize : endTileX;

        startTileY = (int)Mathf.Max(0, startTileY);
        endTileY = (endTileY > CitySize) ? CitySize : endTileY;

        if (spawn)
            Debug.Log("Adding:");
        else
            Debug.Log("Removing:");
        Debug.Log("X: " + startTileX + " -> " + endTileX);
        Debug.Log("Y: " + startTileY + " -> " + endTileY);

        for (int x = startTileX; x < endTileX + 1; x++) {
			for (int y = startTileY; y < endTileY + 1; y++) {
                ChangeTile(spawn, x, y);
            }
		}
	}

    void ChangeTile(bool spawn, int x, int y) {
        if (spawn) { 
            if (CityGameObjects[x, y] == null) { 
                float instPosX = x * TileSize;
                float instPosY = y * TileSize;

                Vector3 basicTilePos = new Vector3(instPosX, -BasicBlockHeight, instPosY);
                Vector3 baseConcretePos = new Vector3(instPosX, 0, instPosY);

                Quaternion instRot = Quaternion.Euler(new Vector3(0, 90 * Random.Range(0, 3), 0));

                CityGameObjects[x, y] = Instantiate(BasicBlock, Vector3.zero + basicTilePos, instRot) as GameObject;
                CityGameObjects[x, y].name = "Tile[" + x + ", " + y + "]";
            }
        } else {
            Destroy(CityGameObjects[x, y]);
        }
    }

    void SetColor(Color color, int x, int y) {
        CityGameObjects[x, y].GetComponentInChildren<Renderer>().material.color = color;
    }
}
