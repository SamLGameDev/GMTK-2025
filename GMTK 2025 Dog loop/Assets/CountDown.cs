using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{

    [SerializeField]
    DogMovement dog;

    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;

    [SerializeField]
    GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dog.CountdownTime - (int)Time.timeSinceLevelLoad <= 0) 
        {
            background.SetActive(false);
            Destroy(gameObject);
        }
         

        textMeshProUGUI.SetText((dog.CountdownTime - (int)Time.timeSinceLevelLoad).ToString());
    }
}
