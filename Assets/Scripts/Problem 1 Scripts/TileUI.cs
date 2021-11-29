using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// UI controllers for an individual tile
/// </summary>
public class TileUI : MonoBehaviour
{
    // The UI image displaying the color
    [SerializeField] Image _tileColor;
    // The TextMeshPro text displaying the number
    [SerializeField] TextMeshProUGUI _tileNumber;

    // the x index of this UI tile
    public int xIndex;
    // the y index of this UI tile
    public int yIndex;

    // the tile this UI displays
    private Tile _tile;

    private void Awake()
    {
        // on awake, subscribe to the button on click event
        GetComponent<Button>().onClick.AddListener(TileClicked);
        // toggle off this UI
        ToggleUI(false);
    }

    public void AssignTile(Tile tile)
    {
        // assign this tile UI
        _tile = tile;
        // update the color and number displayed
        UpdateUIColor(tile.TileColor);
        UpdateUINumber(tile.TileNumber);
    }

    /// <summary>
    /// Updates the UI color displayed
    /// </summary>
    /// <param name="color"></param>
    private void UpdateUIColor(Color color)
    {
        _tileColor.color = color;
    }

    /// <summary>
    /// Updates the UI tile number displayed
    /// </summary>
    /// <param name="number"></param>
    private void UpdateUINumber(int number)
    {
        _tileNumber.text = number.ToString();
    }

    /// <summary>
    /// Toggles this UI tile
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleUI(bool toggle)
    {
        _tileColor.enabled = toggle;
        _tileNumber.enabled = toggle;
    }

    /// <summary>
    /// Callback method evoked when this tile is clicked on
    /// </summary>
    private void TileClicked()
    {
        OnTileClicked.Invoke(this);
    }

    // Event called when this tile UI is clicked on
    [Tooltip("Event called when this tile UI is clicked on")]
    public UnityEvent<TileUI> OnTileClicked = new UnityEvent<TileUI>();
}
