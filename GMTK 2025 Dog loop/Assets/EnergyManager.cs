using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private float winScreenDelay;

    [SerializeField] private FurnitureSet RemainingFurniture;

    private bool callScoreDisplay = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        callScoreDisplay = false ;
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

                foreach (RegisterFurniture furn in RemainingFurniture.GetItems())
                {
                    furn.TriggerScoreDisplay();
                }
            }

            if (winScreenDelay <= 0)
                SceneManager.LoadScene("WinScene");
        }
    }
}
