using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEditor.Rendering.Universal;
using TMPro;
using System.Threading.Tasks;

public class LeaderboardRequests : MonoBehaviour
{

    [SerializeField] private string leaderboardId = "389441346196146146";

    [SerializeField] private Transform LeaderBoardContentParent;

    [SerializeField] private Transform LeaderBoardSlotPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, 0);

        UpdateLeaderBoard();
    }

    private async void UpdateLeaderBoard() 
    {
        while (Application.isPlaying)
        {
           LeaderboardScoresPage leaderboardScoresPage = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);

            for(int i =0; i < leaderboardScoresPage.Total; i++) 
            {
                var entry = leaderboardScoresPage.Results[i];

                Transform display;

                if (i == LeaderBoardContentParent.childCount) 
                {
                    display = Instantiate(LeaderBoardSlotPrefab, LeaderBoardContentParent);
                }
                else 
                {
                    display = LeaderBoardContentParent.GetChild(i);
                }
                display.GetChild(1).GetComponent<TextMeshProUGUI>().text = entry.Score.ToString();
                display.GetChild(2).GetComponent<TextMeshProUGUI>().text = entry.PlayerName.ToString();
            }
            await Task.Delay(1000);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
