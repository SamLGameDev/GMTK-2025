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

                GameObject.FindGameObjectWithTag("Dog").GetComponent<DogMovement>().StopDog();

                totalScoreDisplay.transform.parent.parent.gameObject.SetActive(true);

                foreach (RegisterFurniture furn in RemainingFurniture.GetItems())
                {
                    furn.TriggerScoreDisplay(scoreManager);
                }
            }

            if (winScreenDelay <= 0)
                SceneManager.LoadScene("WinScene");
        }
    }
}
