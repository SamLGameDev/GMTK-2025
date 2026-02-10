using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BoolStore", order = 2)]
public class BoolStore : ScriptableObject
{
    [SerializeField]
    private bool Value;

    public bool GetValue() 
    {
        return Value;   
    }

    public void SetValue(bool value) 
    {
        Value = value;
    }
}
