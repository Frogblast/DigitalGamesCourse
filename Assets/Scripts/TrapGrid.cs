using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
// Class representing the list of gridmaps 
public class GridDataWrapper
{
    public List<GridData> grids; 
}

// Class for the json structure
[System.Serializable]
public class GridData
{
    public int rows;
    public int cols;
    // public int mapID // or something that selects what maps grid is what, todo later
    public int[] map;

    // Due to some fuckery with unity and json it can't directly deserialize 2d maps so the json needs the grid to be one 1d array
    public int[,] GetMap2D()
    {
        int[,] map2D = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                map2D[i, j] = map[i * cols + j];
            }
        }
        return map2D;
    }
}

// Consider renaming this class and refactoring the public variables.
public class TrapGrid : MonoBehaviour
{
        public TextAsset gridDataFile; // Refrence to the json file, added in the inspector 
        public GridDataWrapper gridDataWrapper; // List of maps named gridList in the json

        public GameObject tilePrefab; // Assign a tile prefab in the Inspector
        public float tileSize = 1f; // Size of each tile (adjust if needed)

    void Start()
    { 
        LoadGridData();
        int[,] RandomGrid = SelecteRandomGrid(gridDataWrapper);
        GameObject invGrid = GenerateGrid(RandomGrid); 
        GameObject roofGrid = GenerateGrid(RandomGrid);

        TurnOffMeshRenderer(invGrid);
        roofGrid.transform.localPosition = new Vector3(0, 10f, 0);
    }

        void LoadGridData()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("gridData"); // Load json, Update the JSON to add more maze structures
            if (jsonFile != null)
            {
                gridDataWrapper = JsonUtility.FromJson<GridDataWrapper>(jsonFile.text); // Retrieve the data
            }
            else
            {
                Debug.LogError("grid data json not found");
            }
        }

        GameObject GenerateGrid(int[,] map)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            GameObject gridParent = new GameObject("GeneratedGrid");
            gridParent.transform.SetParent(this.transform);
            gridParent.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
            gridParent.transform.localPosition = Vector3.zero; // zero to the prefab invisiblecorridor

            // Create the grid inside this GameObject selected, in this case the invisiblecorridor prefab
            for (int y = 0; y < map.GetLength(0); y++)
                    {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        if (map[y, x] == 1)
                        {
                            // Make sure the spawn position is relative to the element the script is on
                            Vector3 localPosition = new Vector3(y * tileSize, 0, x * tileSize);

                            GameObject tile = Instantiate(tilePrefab, gridParent.transform);
                        
                            tile.transform.localPosition = localPosition;
                            tile.transform.localScale = new Vector3(tileSize, tileSize, tileSize); // standarise the size of tiles
                        }
                    }
                }
            return gridParent;
        }

        // This runs everytime you hit the lethalzone randomly selecting a new map, noted for now
        int[,] SelecteRandomGrid(GridDataWrapper gridDataWrapper)
        {
            int totalGridCount = gridDataWrapper.grids.Count;
            int randomGridNum = Random.Range(0, totalGridCount);
            int[,] randomSelectedGrid = gridDataWrapper.grids[randomGridNum].GetMap2D();

            Debug.Log("random number: " + randomGridNum);
            return randomSelectedGrid;
        }

        void TurnOffMeshRenderer(GameObject go)
        { 
            for (int i = 0; i < go.transform.childCount; i++)
            { 
                Transform child = go.transform.GetChild(i);
                MeshRenderer childMeshRenderer = child.GetComponentInChildren<MeshRenderer>(); // Structure is GeneratedGrid -> GridCell -> FloorTile
                if (childMeshRenderer != null)                                                 // FloorTile is the one with the MeshRenderer thus GetComponentInChildren
                {
                    childMeshRenderer.enabled = false;   
                }
            }
        }
    }

