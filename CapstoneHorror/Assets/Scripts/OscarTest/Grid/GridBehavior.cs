using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//reference used: https://www.youtube.com/watch?v=fUiNDDcU_I4
//this script will control the AI Pathfinding 

public class GridBehavior : MonoBehaviour
{
    //set size of grid
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridObject;
    //this is where we start generating the grid
    public Vector3 bottomLeft = new Vector3(0,0,0);

    //[,] represents a 2x2 array
    public GameObject[,] gridArray;
    //set up the start and ending points, eventually assign start coordinates as AI and end as Player location
    public int startingX = 0;
    public int startingY = 0;
    public int endingX = 5;
    public int endingY = 5;

    //create a list of gameobject points/nodes making up the shortest path for AI to travel to Player
    public List<GameObject> path = new List<GameObject>();
    public bool findDistance = false;

    public GameObject player;
    public GameObject AI;
    public float speed = 1;
 
    void Awake()
    {
        //create array on start to same size as our grid
        gridArray = new GameObject[columns, rows];

        //create grid on start
        if (gridObject)
        {
            CreateGrid();
        }
        else
        {
            print("Missing a Grid Object, Please Drag and Assign One.");
        }
    }

    void Update()
    {
        if (findDistance)
        {
            //SetDistanceRequired();
            //SetPathToTravel();
            //MoveAI();
            findDistance = false;
        }
    }

    void MoveAI()
    {
        //WIP, idea is to instantia player at bottom left coordinates and have AI travel across the grid points to target Player. Must still link AI transform position to grid array.
        float move = speed * Time.deltaTime;
        player = Instantiate(player, bottomLeft, Quaternion.identity);
        AI.transform.position = Vector3.MoveTowards(AI.transform.position, player.transform.position, move);
    }

    //we generate the actual grid into the scene
    //we iterate through every column and row and instantiate the game object at the initial bottom left node
    void CreateGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                //x and z on 2d plane where z is the y axis, and y is is the up and down direction on 3d plane
                GameObject gridObj = Instantiate(gridObject, new Vector3(bottomLeft.x + scale * i, bottomLeft.y, bottomLeft.z + scale * j), Quaternion.identity);
                gridObj.transform.SetParent(gameObject.transform);

                gridObj.GetComponent<GridInfo>().x = i; //x is columns so = i
                gridObj.GetComponent<GridInfo>().y = j; //y is rows so = j

                //assign array to same grid
                gridArray[i, j] = gridObj;
            }
        }

    }

    //creates grid enabling us to label the points
    void GridInitialSetup()
    {
        foreach (GameObject gameObj in gridArray)
        {
            //here we label every node to a value of -1
            gameObj.GetComponent<GridInfo>().visitedNode = -1;
        }

        //here we label our starting node to a value of 0
        gridArray[startingX, startingY].GetComponent<GridInfo>().visitedNode = 0;
    }

    //x and y is coordinates, steps is # of steps or processes
    bool DirectionCheck(int x, int y, int steps, int direction)
    {
        //  To Help With Visualization, where X is Current Position
        //  [ ]   [ ]   [ ]
        //  [ ]   [X]   [ ]
        //  [ ]   [ ]   [ ]
  
        //int direction will dictate the case direction, where 1 = up, 2 = right, 3 = down, 4 = left
        switch (direction)
        {
            //left direction, check point to left of current
            case 4:
                if (x - 1 < columns && gridArray[x - 1, y] && gridArray[x - 1, y].GetComponent<GridInfo>().visitedNode == steps)
                {
                    return true;
                }
                else return false;

            //down direction, check point below current
            case 3:
                if (y - 1 < rows && gridArray[x, y - 1] && gridArray[x, y - 1].GetComponent<GridInfo>().visitedNode == steps)
                {
                    return true;
                }
                else return false;

            //right direction, check point to right of current
            case 2:
                if (x + 1 < columns && gridArray[x + 1, y] && gridArray[x + 1, y].GetComponent<GridInfo>().visitedNode == steps)
                {
                    return true;
                }
                else return false;

            //up direction, check point above current
            case 1:
                if (y + 1 < rows && gridArray[x, y + 1] && gridArray[x, y + 1].GetComponent<GridInfo>().visitedNode == steps)
                {
                    return true;
                }
                else return false;
        }

        return false;
    }

    //set different coordinate positions to our visited values 
    void SetVisitedNode(int x, int y, int steps)
    {
        if (gridArray[x,y])
        {
            gridArray[x, y].GetComponent<GridInfo>().visitedNode = steps;
        }
    }

    //set the distance based on how far it is from starting point
    void SetDistanceRequired()
    {
        GridInitialSetup();
        int x = startingX;
        int y = startingY;
        //we multiply rows * columns to ensure it is greater than any movement we can possibly move
        int[] array = new int[rows * columns];

        //go through as many steps as possible to ensure we check all available routes to destination
        for (int steps = 1; steps < rows * columns; steps++)
        {
            //go through entire grid array and check through every available grid object
            foreach(GameObject gameObj in gridArray)
            {
                if (gameObj && gameObj.GetComponent<GridInfo>().visitedNode == steps - 1)
                {
                    //check surrounding nodes to see if -1 and set to new value
                    FourDirectionsCheck(gameObj.GetComponent<GridInfo>().x, gameObj.GetComponent<GridInfo>().y, steps);
                }
            }
        }
    }

    //starting at initial node value 0, we check surrounding nodes to see if value of -1 and update their values according to # of steps its been
    void FourDirectionsCheck(int x, int y, int steps)
    {
        //check up direction node
        if (DirectionCheck(x, y, -1, 1))
        {
            SetVisitedNode(x, y + 1, steps);
        }

        //check right direction node
        if (DirectionCheck(x, y, -1, 2))
        {
            SetVisitedNode(x + 1, y, steps);
        }

        //check down direction node
        if (DirectionCheck(x, y, -1, 3))
        {
            SetVisitedNode(x, y - 1, steps);
        }

        //check left direction node
        if (DirectionCheck(x, y, -1, 4))
        {
            SetVisitedNode(x - 1, y, steps);
        }
    }

    //here we find and set the path for AI to travel to destination
    void SetPathToTravel()
    {
        int steps;
        int x = endingX;
        int y = endingY;
        List<GameObject> list = new List<GameObject>();
        path.Clear();

        //starting from ending node, we add the path required back to start, hence the step - 1 until we end up back at 0, the initial point
        if (gridArray[endingX, endingY] && gridArray[endingX, endingY].GetComponent<GridInfo>().visitedNode > 0)
        {
            path.Add(gridArray[x, y]);
            steps = gridArray[x, y].GetComponent<GridInfo>().visitedNode - 1;
        }
        else
        {
            print("Can't Reach The Desired Location.");
            return;
        }

        //starting at last point and moving down to 0
        for (int i = steps; steps > -1; steps--)
        {
            //if node to travel is above, add to the list to travel
            if (DirectionCheck(x, y, steps, 1))
            {
                list.Add(gridArray[x, y + 1]);
            }

            //if node to travel is right, add to the list to travel
            if (DirectionCheck(x, y, steps, 2))
            {
                list.Add(gridArray[x + 1, y]);
            }

            //if node to travel is below, add to the list to travel
            if (DirectionCheck(x, y, steps, 3))
            {
                list.Add(gridArray[x, y - 1]);
            }

            //if node to travel is left, add to the list to travel
            if (DirectionCheck(x, y, steps, 4))
            {
                list.Add(gridArray[x - 1, y]);
            }

            GameObject obj = FindClosestPath(gridArray[endingX, endingY].transform, list);
            path.Add(obj);
            x = obj.GetComponent<GridInfo>().x;
            y = obj.GetComponent<GridInfo>().y;
            list.Clear();
        }
    }

    //here we find and set the closest path for AI to travel to destination
    GameObject FindClosestPath(Transform endLocation, List<GameObject> list)
    {
        float currentDistance = scale * rows * columns;
        int index = 0;

        //check which list has shortest distance, and set that as path to travel
        for (int i = 0; i < list.Count; i++)
        {
            if (Vector3.Distance(endLocation.position, list[i].transform.position) < currentDistance)
            {
                currentDistance = Vector3.Distance(endLocation.position, list[i].transform.position);
                index = i;
            }
        }
        return list[index];
    }
}
