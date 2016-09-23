using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RoadGenerator : MonoBehaviour {

    int CitySize;
    public int DistanceBetweenRoads;
    Road startRoad;
    Direction[,] city;
    CityGenerator cityGenerator;
    List<Road> queue;


    enum RoadType { Highway, Regular };
    enum Direction { North = 1, East = 2, South = 3, West = 4 };


    class RoadShape
    {
        public int[] start;
        public int[] end;
        public Direction direction;
        public int length;
        public RoadShape(int[] startPos, int[] endPos, Direction dir, int len)
        {
            start = startPos;
            end = endPos;
            direction = dir;
            length = len;
        }
    }

    class Road
    {
        public int prio;
        public RoadShape shape;
        public RoadType type;
        public Road(int p, RoadShape rs, RoadType rt)
        {
            prio = p;
            shape = rs;
            type = rt;
        }
    }

    void Start()
    {
        queue = new List<Road>();
        cityGenerator = GetComponent<CityGenerator>();
        city = new Direction[cityGenerator.CitySize, cityGenerator.CitySize];
    }

	
	public void Init () {
        queue.Add(GetStartRoad());
        while(queue.Count > 0)
        {
            int min = queue.Min(entry => entry.prio);
            Road nextRoad = queue.First(r => r.prio == min);
            queue.Remove(nextRoad);
            InsertRoad(nextRoad);
        }
    }

    Road GetStartRoad()
    {
        int length = Random.Range(3, 10), pos = Random.Range(0, cityGenerator.CitySize - 1);
        int[] start = new int[2], end = new int[2];
        Direction dir = (Direction)Random.Range(1, 5);
        Debug.Log(dir);
        switch (dir)
        {
            case (Direction.North):
                start[0] = pos;
                start[1] = 0;
                end[0] = pos;
                end[1] = length;
                break;

            case (Direction.East):
                start[0] = 0;
                start[1] = pos;
                end[0] = length;
                end[1] = pos;
                break;

            case (Direction.South):
                start[0] = pos;
                start[1] = cityGenerator.CitySize - 1;
                end[0] = pos;
                end[1] = cityGenerator.CitySize - 1 - length;
                break;

            case (Direction.West):
                start[0] = cityGenerator.CitySize - 1;
                start[1] = pos;
                end[0] = cityGenerator.CitySize - 1 - length;
                end[1] = pos;
                break;
        }
        RoadShape rs = new RoadShape(start, end, dir, length);
        Road r = new Road(0, rs, RoadType.Highway);
        InsertRoad(r);
        return r;
    }
	
    bool LocalConstraints(Road road)
    {
        return true;
    }

    bool InsertRoad(Road r)
    {
        RoadShape rs = r.shape;
        if (rs.direction == Direction.North || rs.direction == Direction.South)
        {
            int from = rs.start[1];
            int to = from + rs.length;
            if(rs.direction == Direction.South)
            {
                from = rs.start[1] - rs.length;
                to = rs.start[1];
            }
            for(int y=from;y<to+1;y++)
            {
                if (0 > y || y > cityGenerator.CitySize - 1)
                    return false;
                if (city[rs.start[1], y] != 0)
                    return false;

                for (int xOffset = -DistanceBetweenRoads; xOffset < DistanceBetweenRoads + 1; xOffset++)
                {
                    if (0 < rs.start[1] + xOffset && rs.start[1] + xOffset < cityGenerator.CitySize)
                    {
                        if (city[rs.start[1] + xOffset, y] == Direction.North || city[rs.start[1] + xOffset, y] == Direction.South)
                        {
                            return false;
                        }
                    }
                   
                }
                cityGenerator.SetDistrict(CityGenerator.District.Road, Color.black, rs.start[0], y);
                city[rs.start[0], y] = rs.direction;
            }
        }
        else
        {
            int from = rs.start[0];
            int to = from + rs.length;
            if (rs.direction == Direction.West)
            { 
                from = rs.start[0] - rs.length;
                to = rs.start[0];
            }
            for (int x = from; x < to+1; x++)
            {
                if (0 > x || x > cityGenerator.CitySize - 1)
                    return false;
                if (city[x, rs.start[1]] != 0)
                    return false;

                for (int yOffset = -DistanceBetweenRoads; yOffset < DistanceBetweenRoads + 1; yOffset++)
                {
                    if (0 < rs.start[1] + yOffset && rs.start[1] + yOffset < cityGenerator.CitySize)
                    {
                        if (city[x, rs.start[1] + yOffset] == Direction.East || city[x, rs.start[1] + yOffset] == Direction.West)
                        {
                            return false;
                        }
                    }
                    
                }
                cityGenerator.SetDistrict(CityGenerator.District.Road, Color.black,x, rs.start[1]);
                city[x, rs.start[1]] = rs.direction;
            }
        }
        GlobalGoals(r);
        return true;
    }

    void GlobalGoals(Road r)
    {
        RoadShape rs = r.shape;
        for(int dir=1;dir<5;dir++)
        {
            if((Direction)dir != oppositeDirection(rs.direction))
            {
                int length = Random.Range(3, 10);
                int[] start = new int[2], end = new int[2];
                switch ((Direction)dir)
                {
                    case (Direction.North):
                        start[0] = rs.end[0];
                        start[1] = rs.end[1] + 1;
                        end[0] = rs.end[0];
                        end[1] = rs.end[1] + 1 + length;
                        break;

                    case (Direction.East):
                        start[0] = rs.end[0] + 1;
                        start[1] = rs.end[1];
                        end[0] = rs.end[0] + 1 + length;
                        end[1] = rs.end[1];
                        break;

                    case (Direction.South):
                        start[0] = rs.end[0];
                        start[1] = rs.end[1] - 1;
                        end[0] = rs.end[0];
                        end[1] = rs.end[1] - 1 - length;
                        break;

                    case (Direction.West):
                        start[0] = rs.end[0] - 1;
                        start[1] = rs.end[1];
                        end[0] = rs.end[0] - 1 - length;
                        end[1] = rs.end[1];
                        break;
                }
                RoadShape suggestedShape = new RoadShape(start, end, (Direction)dir, length);
                Road suggestedRoad = new Road(r.prio * 4 + dir, suggestedShape, RoadType.Regular);
                queue.Add(suggestedRoad);
            }
        }        
    }

    Direction oppositeDirection(Direction dir)
    {
        Direction oppositeDir = dir;
        switch (dir)
        {
            case (Direction.North):
                oppositeDir = Direction.South;
                break;

            case (Direction.East):
                oppositeDir = Direction.West;
                break;

            case (Direction.South):
                oppositeDir = Direction.North;
                break;

            case (Direction.West):
                oppositeDir = Direction.East;
                break;
        }
        return oppositeDir;
    }
}
