using Cysharp.Threading.Tasks;
using Gameplay.Furniture;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    [SerializeField]
    float MaxEnergy;

    [SerializeField]
    Image EnergyMeter;

    [SerializeField]
    float Speed;

    [SerializeField]
    GameObjectStore DogStore;

    private ScoreManager scoreManager;

    [SerializeField] private float winScreenDelay;

    [SerializeField] private FurnitureSet RemainingFurniture;

    private bool callScoreDisplay = false;

    private TextMeshProUGUI totalScoreDisplay;
    private float totalScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        callScoreDisplay = false ;
        totalScoreDisplay = GameObject.FindGameObjectWithTag("TotalScore").GetComponent<TextMeshProUGUI>();
        totalScoreDisplay.transform.parent.parent.gameObject.SetActive(false);
        scoreManager = GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    async void Update()
    {

        if (DogStore.GetObject() != null && DogStore.GetObject().GetComponent<DogMovement>().countDown) 
        {
            return;
        }

        EnergyMeter.fillAmount = EnergyMeter.fillAmount - Speed * Time.deltaTime / MaxEnergy;

        if (EnergyMeter.fillAmount <= 0)
        {
            winScreenDelay -= Time.deltaTime;

            if (!callScoreDisplay)
            {
                callScoreDisplay = true;

               // GameObject.FindGameObjectWithTag("Dog").GetComponent<DogMovement>().StopDog();

                totalScoreDisplay.transform.parent.parent.gameObject.SetActive(true);

                foreach (RegisterFurniture fur in RemainingFurniture.GetItems())
                {
                    totalScore += fur.SetCountDown();
                }

                TriggerScoreDisplay();
            }

            if (winScreenDelay <= 0)
                SceneManager.LoadScene("WinScene");
        }
    }

    private async UniTask TriggerScoreDisplay()
    {
        int i = 0;
        RegisterFurniture currentFur = RemainingFurniture.GetItemAtIndex(i);

        while (true)
        {
            foreach (RegisterFurniture fur in RemainingFurniture.GetItems())
            {
                if (fur.scoreCountDown > 0)
                {
                    fur.GiveScoreToLeader();
                    scoreManager.IncreaseTotalScore(1);
                    totalScore -= 1;
                }
            }

            if (totalScore <= 0)
            {
                CancelInvoke();
                break;
            }

            if (winScreenDelay <= 0)
            {
                SceneManager.LoadScene("WinScene");
                CancelInvoke();
                break;
            }

            await UniTask.WaitForSeconds(0.1f);
        }

    }
}

