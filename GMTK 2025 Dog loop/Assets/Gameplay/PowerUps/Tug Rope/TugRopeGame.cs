using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using Unity.Jobs;
using UnityEngine.UI;

using UnityEngine;

public class TugRopeGame : MonoBehaviour
{
    [SerializeField] private GameObject Canvas;

    [SerializeField] private Image PowerProgess;

    [SerializeField] private float DogPower;
    [SerializeField] private float PlayerPower;

    private CancellationTokenSource DogPowerCTS;

    private CancellationTokenSource MiniGameDurationCTS;

    [SerializeField] private GameEvent MiniGameEnded;

    [SerializeField] private float MiniGameDuration;

    private float RemainingTime;

    [SerializeField] private TextMeshProUGUI RemainingTimeText;


    public void StartMiniGame()
    {
        Canvas.SetActive(true);

        DogPowerCTS = new CancellationTokenSource();
        AddDogPower(DogPowerCTS.Token);

        RemainingTime = MiniGameDuration;
        MiniGameDurationCTS = new CancellationTokenSource();
        MiniGameDurationUpdater(MiniGameDurationCTS.Token);

    }

    public async UniTask MiniGameDurationUpdater(CancellationToken Token)
    {
        while (true)
        {
            if (Token.IsCancellationRequested) return;

            RemainingTime -= Time.fixedDeltaTime;

            int timeRemaining = (int)RemainingTime;

            RemainingTimeText.text = timeRemaining.ToString();

            if (RemainingTime < 0)
            {
                EndMiniGame();
                return;
            }

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }
    }

    public async UniTask AddDogPower(CancellationToken Token)
    {
        while (true)
        {
            if (Token.IsCancellationRequested)
            {
                return;
            }

            PowerProgess.fillAmount -= DogPower * Time.fixedDeltaTime;

            if (PowerProgess.fillAmount <= 0)
            {
                EndMiniGame();
                return;
            }

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }
    }

    public void AddPlayerPower()
    {
        PowerProgess.fillAmount += PlayerPower;
        if (PowerProgess.fillAmount >= 1)
        {
            EndMiniGame();
            return;
        }
    }

    public void EndMiniGame()
    {
        Canvas.SetActive(false);
        DogPowerCTS.Cancel();
        MiniGameDurationCTS.Cancel();
        MiniGameEnded.Raise();

    }

}
