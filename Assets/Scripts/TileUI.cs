using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class TileUI : MonoBehaviour
{
    [SerializeField] Image _tileColor;
    [SerializeField] TextMeshProUGUI _tileNumber;

    public int xIndex;
    public int yIndex;

    private Tile _tile;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(TileClicked);
        ToggleUI(false);
    }

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

    private void TileClicked()
    {
        OnTileClicked.Invoke(this);
    }


    public UnityEvent<TileUI> OnTileClicked = new UnityEvent<TileUI>();
}
