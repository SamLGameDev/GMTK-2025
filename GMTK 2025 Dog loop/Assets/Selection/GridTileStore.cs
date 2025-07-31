using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GridTileStore", order = 1)]
public class GridTileStore : ScriptableObject
{
    [SerializeField]
    private List<GridTile> gridTiles;

    public List<GridTile> GetTiles() 
    {
        return gridTiles;
    }

    public void SetTiles(List<GridTile> tiles)
    {
        gridTiles.Clear();
        gridTiles = tiles;
    }

    public void Clear() 
    {
        gridTiles.Clear();  
    }
}
