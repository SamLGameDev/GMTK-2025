using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FurnitureStore", order = 1)]
public class FurnitureStore : ScriptableObject
{
    [SerializeField]
    private Furniture ObjectTiles;

    public bool Blocked;

    public Furniture GetObject()
    {
        return ObjectTiles;
    }

    public void SetObjects(Furniture tiles)
    {
        ObjectTiles = tiles;
    }

}