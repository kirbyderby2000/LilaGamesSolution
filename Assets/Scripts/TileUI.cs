using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TileUI : MonoBehaviour
{
    [SerializeField] Image _tileColor;
    [SerializeField] TextMeshProUGUI _tileNumber;

    private Tile _tile;
    
    public void AssignTile(Tile tile)
    {
        _tile = tile;
        UpdateUIColor(tile.TileColor);
        UpdateUINumber(tile.TileNumber);
    }

    private void UpdateUIColor(Color color)
    {
        _tileColor.color = color;
    }

    private void UpdateUINumber(int number)
    {
        _tileNumber.text = number.ToString();
    }

    public void ToggleUI(bool toggle)
    {
        _tileColor.enabled = toggle;
        _tileNumber.enabled = toggle;
    }
}
