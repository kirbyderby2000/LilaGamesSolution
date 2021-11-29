using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Tile manager responsible for controlling a grid, instantiating tiles in the grid, and responding to the area of interest selection
public class TileManager : MonoBehaviour
{
    [Header("Game Object References")]
    [SerializeField, Tooltip("The tile UI prefab to clone within the grid")] TileUI _tilePrefab;
    [SerializeField, Tooltip("The root UI canvas in the scene")] RectTransform rootCanvas;
    [Header("Data Generation, Grid, and Tile Configuration")]
    [SerializeField, Tooltip("How many tiles you would like to generate.")] int _nTiles = 50;
    [SerializeField, Min(0), Tooltip("How extending bounds in cells of the area of interest.")] int _areaOfInterestSize = 1;
    [SerializeField, Min(1), Tooltip("How many rows should populate the grid.")] private int _gridRows = 10;
    // cached grid colums
    private int _gridColums = 5;
    // cached grid tiles (2D array)
    TileUI[,] _gridTiles = new TileUI[0,0];

    private void Awake()
    {
        PopulateGrid();
    }

    // returns a calculated grid cell size to perfectly fit the screen with the configured grid dimensions
    private Vector2 CalculateGridCellSize()
    {
        // if there are less tiles than grid rows assigned, log a warning and reassign the grid rows to N
        if(_nTiles < _gridRows)
        {
            Debug.LogWarning("You've specified more rows than tiles desired. Readjusting rows to N and columns to 1");
            _gridRows = _nTiles;
        }
        // calculate how many colums are needed to populate the grid with the given rows
        // ceiling is used in the event that N has a modulus greater than 1 to the grid rows (ie 15 tiles and 10 rows will require 2 colums instead of 1)
        _gridColums = Mathf.CeilToInt(_nTiles / (float) _gridRows);
        // calculate the cell width and height for the referenced resolution (1080 x 1920)
        float width = 1080.0f / _gridColums;
        float height = ((float) 1920.0f) /  _gridRows;
        // scale the height of the cells to the actual screen
        height *= (rootCanvas.sizeDelta.y) / 1920.0f;
        return new Vector2(width, height);
    }



    private void PopulateGrid()
    {
        // get the top left corner cell position
        Vector2 topLeftCornerCell = Camera.main.ViewportToScreenPoint(new Vector3(0.0f, 1.0f, 0.0f));
        // calculate the grid cell size
        Vector2 gridCellSize = CalculateGridCellSize();
        // initialize the 2d array to the grid dimensions assigned
        _gridTiles = new TileUI[_gridRows, _gridColums];
        // initialize a tile counter
        int tileCount = 0;

        for (int rows = 0; rows < _gridRows && tileCount < _nTiles; rows++)
        {
            for (int cols = 0; cols < _gridColums && tileCount < _nTiles; cols++)
            {
                // instantiate a tile UI
                TileUI instantiatedTile = Instantiate(_tilePrefab, topLeftCornerCell, _tilePrefab.transform.rotation, transform);
                // assign the tile reference value to the UI
                instantiatedTile.AssignTile(new Tile(tileCount + 1));
                // subscribe to the on UI click event
                instantiatedTile.OnTileClicked.AddListener(OnTileClicked);
                // resize the UI to fit the screen precisely
                RectTransform tileRectTrans = instantiatedTile.GetComponent<RectTransform>();
                tileRectTrans.sizeDelta = gridCellSize;
                // offset the UI to into its unique cell position
                Vector2 anchoredPosition = tileRectTrans.anchoredPosition;
                anchoredPosition.x += gridCellSize.x * cols;
                anchoredPosition.y -= gridCellSize.y * rows;
                tileRectTrans.anchoredPosition = anchoredPosition;
                _gridTiles[rows, cols] = instantiatedTile;
                // assign the x and y indices to the UI
                instantiatedTile.xIndex = rows;
                instantiatedTile.yIndex = cols;
                // increment the tile counter
                tileCount++;
            }
        }
    }

    private void OnTileClicked(TileUI tileClicked)
    {
        OpenAreaOfInterest(tileClicked.xIndex - _areaOfInterestSize, tileClicked.yIndex - _areaOfInterestSize, tileClicked.xIndex + _areaOfInterestSize, tileClicked.yIndex + _areaOfInterestSize);
    }


    // opens an area of interest starting from the starting x & y indices to the ending x & y indices 
    private void OpenAreaOfInterest(int startX, int startY, int endX, int endY)
    {
        { }

        startX = Mathf.Max(startX, 0);
        endX = Mathf.Min(endX, _gridTiles.GetLength(0) - 1);
        startY = Mathf.Max(startY, 0);
        endY = Mathf.Min(endY, _gridTiles.GetLength(1) - 1);

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if(_gridTiles[x,y] != null)
                {
                    _gridTiles[x, y].ToggleUI(true);
                }
            }
        }
    }


}
