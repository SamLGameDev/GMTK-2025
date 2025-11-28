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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (DogStore.GetObject() != null && DogStore.GetObject().GetComponent<DogMovement>().countDown) 
        {
            return;
        }

        EnergyMeter.fillAmount = EnergyMeter.fillAmount - Speed * Time.deltaTime / MaxEnergy;

        if (EnergyMeter.fillAmount <= 0)
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}
