using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FloatStore", order = 1)]
public class FloatStore : ScriptableObject
{
    [SerializeField]
    private float Value;

    public float GetValue() 
    {
        return Value;   
    }

    public void SetValue(float value) 
    {
        Value = value;
    }
}
