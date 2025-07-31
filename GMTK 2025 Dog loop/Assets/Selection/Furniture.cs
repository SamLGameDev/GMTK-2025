using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Furniture", order = 1)]
public class Furniture : ScriptableObject
{
    public GameObject furniture;

    public Vector2 Size;

}
