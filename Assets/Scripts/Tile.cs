using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public int TileNumber
    {
        private set;
        get;
    }

    public Color TileColor
    {
        private set;
        get;
    }

    public Tile(int tileNumber)
    {
        TileNumber = tileNumber;
        TileColor = new Color(Random.value, Random.value, Random.value, 1.0f);
    }
}
