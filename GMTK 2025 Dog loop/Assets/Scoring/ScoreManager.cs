using System;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private FurnitureSet RemainingFurniture;

    [SerializeField]
    private FloatStore TotalScore;

    [SerializeField]
    private string LeaderBoardID;

    private void Start()
    {
        TotalScore.SetValue(0f);
    }

    private async void OnDestroy()
    {
        foreach (RegisterFurniture furn in RemainingFurniture.GetItems())
        {
            TotalScore.SetValue(furn.furniture.GetScore() + TotalScore.GetValue());
        }

        if (!AuthenticationService.Instance.IsSignedIn) 
        {
            return;
        }

        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderBoardID, TotalScore.GetValue());
    }
}
