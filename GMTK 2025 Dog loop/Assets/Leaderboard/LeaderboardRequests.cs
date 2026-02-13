using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LeaderboardRequests : MonoBehaviour
{

    [SerializeField] private string leaderboardId = "389441346196146146";

    [SerializeField] private Transform LeaderBoardContentParent;

    [SerializeField] private Transform LeaderBoardSlotPrefab;

    public async void Awake()
    {
        if (FindAnyObjectByType<AccountManager>()) 
        {
            UpdateLeaderBoard();
            return;
        }

        await SceneManager.LoadSceneAsync("AccountSignIn", LoadSceneMode.Additive);

    }

    public void StartLeaderboardUpdate() 
    {
        UpdateLeaderBoard();
    }
    public async void UpdateLeaderBoard() 
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

                string playerName = entry.PlayerName.Split('#')[0];

                display.GetChild(2).GetComponent<TextMeshProUGUI>().text = playerName;
            }
            await Task.Delay(1000);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
