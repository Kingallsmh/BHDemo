using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {

    [Header("Map Settings")]
    public int mapWidth;
    public int mapHeight;
    Tilemap map;

    [Header("Main Hallway Settings")]
    public int hallwayWidthMin;
    public int hallwayWidthMax;
    public int hallwayHeightMin;
    public int hallwayHeightMax;

    [Header("Sub Hallway Settings")]
    public int numOfSubHallsMin;
    public int numOfSubHallsMax;
    public int subHallWidthMin;
    public int subHallWidthMax;
    public int subHallHeightMin;
    public int subHallHeightMax;

    public void StartGeneration(){
        
    }

    public void CreateMainRoom(){
        
    }

    //Use this to push the generation towards a general direction
    public void LeanGenerationInDirection(){
        
    }

    //Create the hallway to go around the main room so it doesn't intersect
    public void WorkAroundRoom(){
        
    }

    //Check to see if the hallway is trying to go outside the boundaries
    public void BoundaryCheck(){
        
    }


}
