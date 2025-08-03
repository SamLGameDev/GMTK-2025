using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameObjectStore", order = 1)]
public class GameObjectStore : ScriptableObject
{
    [SerializeField]
    private GameObject ObjectTiles;

    public bool Blocked;

    public List<GameObject> Blocking;

    public GameObject GetObject()
    {
        return ObjectTiles;
    }

    public void SetObjects(GameObject tiles)
    {
        ObjectTiles = tiles;
    }

}
