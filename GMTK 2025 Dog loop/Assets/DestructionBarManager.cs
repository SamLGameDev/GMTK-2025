using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DestructionBarManager : MonoBehaviour
{
    [SerializeField]
    Image DestructionBar;

    [SerializeField]
    FurnitureSet CurrentFurniture;

    [SerializeField]
    float FillSpeed;

    int MaxFurniture;


    [SerializeField]
    FloatStore RemainingDestructionAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MaxFurniture = CurrentFurniture.GetListSize();
    }

    // Update is called once per frame
    void Update()
    {
        float CurrentFillAmount = Mathf.MoveTowards(DestructionBar.fillAmount, (float)CurrentFurniture.GetListSize() / MaxFurniture, FillSpeed * Time.deltaTime);

        DestructionBar.fillAmount = CurrentFillAmount;


        RemainingDestructionAmount.SetValue(CurrentFillAmount);

        if (DestructionBar.fillAmount <= 0)
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }
}
