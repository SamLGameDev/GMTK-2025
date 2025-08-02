using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameObjectStore", order = 1)]
public class GameObjectStore : ScriptableObject
{
    [SerializeField]
    private GameObject ObjectTiles;


    public bool Right;

    public bool Left;

    public bool Down;

    public bool Up;

    public bool Blocked;

    public GameObject GetObject()
    {
        return ObjectTiles;
    }

    public void SetObjects(GameObject tiles)
    {
        ObjectTiles = tiles;
    }

}
