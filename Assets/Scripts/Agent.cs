using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Agent : MonoBehaviour {

    CityGenerator.District[,] City;
    int CitySize;
    int TileSize;
    
    public void Initialize(CityGenerator.District[,] city, int citySize, int tileSize, int startX, int startY) {
        City = city;
        CitySize = citySize;
        TileSize = tileSize;
        //FindPath(startX, startY, Random.Range(0, citySize), Random.Range(0, citySize));
    }

    public void FindPath(int startX, int startY, int targetX, int targetY) {
        PathFindingNode startNode = new PathFindingNode(true, startX, startY);
        PathFindingNode targetNode = new PathFindingNode(true, targetX, targetY);

        List<PathFindingNode> openSet = new List<PathFindingNode>();
        HashSet<PathFindingNode> closedSet = new HashSet<PathFindingNode>();

        openSet.Add(startNode);

        while (openSet.Count > 0) {
            PathFindingNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                return;
            }

            foreach (PathFindingNode neighbour in GetNeighbours(currentNode)) {

            }

        }
    }

    PathFindingNode[] GetNeighbours(PathFindingNode node) {
        List<PathFindingNode> neighbours = new List<PathFindingNode>();
        int newX = Mathf.Min(node.X + 1, CitySize);
        int newY = node.Y;
        if (City[newX, newY] == CityGenerator.District.Road) {

        }
        return null;
    }
}
