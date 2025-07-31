using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameObjectStore", order = 1)]
public class GameObjectStore : ScriptableObject
{
    [SerializeField]
    private List<GameObject> ObjectTiles;

    public List<GameObject> GetObject()
    {
        return ObjectTiles;
    }

    public void SetObjects(List<GameObject> tiles)
    {
        ObjectTiles.Clear();
        ObjectTiles = tiles;
    }

    public void Clear()
    {
        ObjectTiles.Clear();
    }
}
