using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardRequests : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public struct LeaderboardData
    {
        public string Name;
        public int Score;
    }

    private LeaderboardData[] leaderboardDatas;

    public void PostScore(LeaderboardData data)
    {
        string SONData = JsonUtility.ToJson(data);
        byte[] raw = System.Text.Encoding.UTF8.GetBytes(SONData);

        UnityWebRequest request;
        if (Array.Exists(leaderboardDatas, name => name.Name == data.Name))
        {
            foreach (var entry in leaderboardDatas)
            {
                if (entry.Name == data.Name)
                {
                    if (data.Score > entry.Score)
                    {
                        request = UnityWebRequest.Put("https://example.com/leaderboard/update", raw);
                        request.SetRequestHeader("Content-Type", "application/json");
                        request.method = UnityWebRequest.kHttpVerbPUT;
                        request.SendWebRequest();
                    }
                    return;
                }
            }
        }
        request = UnityWebRequest.Post("https://example.com/leaderboard/add", SONData);
        request.SetRequestHeader("Content-Type", "application/json");
        request.method = UnityWebRequest.kHttpVerbPOST;
        request.SendWebRequest();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
