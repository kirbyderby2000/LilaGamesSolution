using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the tile
/// </summary>
public class Tile
{
    /// <summary>
    /// The number of this tile
    /// </summary>
    public int TileNumber
    {
        private set;
        get;
    }

    /// <summary>
    /// The randomized color of this tile
    /// </summary>
    public Color TileColor
    {
        private set;
        get;
    }

    public Tile(int tileNumber)
    {
        // When this tile is constructed, assign the number and randomize the color value
        TileNumber = tileNumber;
        TileColor = new Color(Random.value, Random.value, Random.value, 1.0f);
    }
}
