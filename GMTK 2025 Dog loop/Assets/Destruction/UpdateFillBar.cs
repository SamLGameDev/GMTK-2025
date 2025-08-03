using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField]
    private FloatStore amount;

    [SerializeField]
    Image fillBar;
    // Start is called before the first frame update
    void Start()
    {
        fillBar.fillAmount = amount.GetValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
