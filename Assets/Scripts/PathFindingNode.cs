using UnityEngine;
using System.Collections;

public class PathFindingNode {

    public bool Walkable;
    public int X;
    public int Y;

    public int GCost;
    public int HCost;

    public PathFindingNode(bool walkable, int x, int y) {
        Walkable = walkable;
        X = x;
        Y = y;
    }

    public int FCost {
        get {
            return GCost + HCost;
        }
    }
}
