using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private FurnitureSet RemainingFurniture;

    [SerializeField]
    private FloatStore TotalScore;

    private void Start()
    {
        TotalScore.SetValue(0f);
    }

    private void OnDestroy()
    {
        foreach (RegisterFurniture furn in RemainingFurniture.GetItems())
        {
            TotalScore.SetValue(furn.furniture.GetScore() + TotalScore.GetValue());
        }
        if (TotalScore.GetValue() > PlayerPrefs.GetFloat("PersonalHighScore"))
        {
            PlayerPrefs.SetFloat("PersonalHighScore", TotalScore.GetValue());
        }

    }
}
