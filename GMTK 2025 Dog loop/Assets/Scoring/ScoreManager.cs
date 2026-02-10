using Cysharp.Threading.Tasks;
using System;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private FurnitureSet RemainingFurniture;

    [SerializeField]
    private FloatStore TotalScore;

    [SerializeField]
    private string LeaderBoardID;

    private TextMeshProUGUI totalScoreDisplay;

    private void Awake()
    {
        totalScoreDisplay = GameObject.FindGameObjectWithTag("TotalScore").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        TotalScore.SetValue(0f);
        totalScoreDisplay = GameObject.FindGameObjectWithTag("TotalScore").GetComponent<TextMeshProUGUI>();
    }

    private void FindTotalScore()
    {
        foreach (RegisterFurniture furn in RemainingFurniture.GetItems())
        {
            TotalScore.SetValue(furn.furniture.GetScore() + TotalScore.GetValue());
        }
    }

    public void IncreaseTotalScore(float increaseBy)
    {
        TotalScore.SetValue(increaseBy + TotalScore.GetValue());
        totalScoreDisplay.text = TotalScore.GetValue().ToString();
    }

    private async void OnDestroy()
    {
        FindTotalScore();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            return;
        }

        await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderBoardID, TotalScore.GetValue());
    }
}
