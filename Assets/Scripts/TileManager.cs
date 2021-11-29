using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    [SerializeField] TileUI _tilePrefab;
    [SerializeField] Transform _gridParent;
    [SerializeField] RectTransform rootCanvas;
    [SerializeField] int _nTiles = 50;
    [SerializeField, Min(0)] int _areaOfInterestSize = 1;
    [SerializeField, Min(1)] private int _gridRows = 10;
    private int _gridColums = 5;
    TileUI[,] _gridTiles = new TileUI[0,0];

    private void Awake()
    {
        PopulateGrid();
    }


    private Vector2 CalculateGridCellSize()
    {
        if(_nTiles < _gridRows)
        {
            Debug.LogWarning("You've specified more rows than tiles desired. Readjusting rows to N and columns to 1");
            _gridRows = _nTiles;
        }
        _gridColums = Mathf.CeilToInt(_nTiles / (float) _gridRows);
        float width = 1080.0f / _gridColums;
        float height = ((float) 1920.0f) /  _gridRows;
        height *= (rootCanvas.sizeDelta.y) / 1920.0f;
        return new Vector2(width, height);
    }



    private void PopulateGrid()
    {
        Vector2 topLeftCornerCell = Camera.main.ViewportToScreenPoint(new Vector3(0.0f, 1.0f, 0.0f));
        Vector2 gridCellSize = CalculateGridCellSize();
        _gridTiles = new TileUI[_gridRows, _gridColums];
        int tileCount = 0;
        for (int rows = 0; rows < _gridRows && tileCount < _nTiles; rows++)
        {
            for (int cols = 0; cols < _gridColums && tileCount < _nTiles; cols++)
            {
                TileUI instantiatedTile = Instantiate(_tilePrefab, topLeftCornerCell, _tilePrefab.transform.rotation, _gridParent);
                instantiatedTile.AssignTile(new Tile(tileCount + 1));
                instantiatedTile.OnTileClicked.AddListener(OnTileClicked);
                RectTransform tileRectTrans = instantiatedTile.GetComponent<RectTransform>();
                tileRectTrans.sizeDelta = gridCellSize;
                Vector2 anchoredPosition = tileRectTrans.anchoredPosition;
                anchoredPosition.x += gridCellSize.x * cols;
                anchoredPosition.y -= gridCellSize.y * rows;
                tileRectTrans.anchoredPosition = anchoredPosition;
                tileCount++;
                _gridTiles[rows, cols] = instantiatedTile;
                instantiatedTile.xIndex = rows;
                instantiatedTile.yIndex = cols;
            }
        }
    }

    private void OnTileClicked(TileUI tileClicked)
    {
        OpenAreaOfInterest(tileClicked.xIndex - _areaOfInterestSize, tileClicked.yIndex - _areaOfInterestSize, tileClicked.xIndex + _areaOfInterestSize, tileClicked.yIndex + _areaOfInterestSize);
    }


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
