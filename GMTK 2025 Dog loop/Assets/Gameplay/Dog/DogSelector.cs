
using UnityEngine;
using UnityEngine.UI;

public class DogSelector : MonoBehaviour
{

    [SerializeField] private GameObjectSet Dogs;

    [SerializeField] private GameObjectStore CurrentDog;

    [SerializeField] private Image DogImage;

    private int index;

    void Start()
    {
        if (!CurrentDog.GetObject())
        {
            index = 0;
            CurrentDog.SetObjects(Dogs.GetItemAtIndex(0));
        }

        index = Dogs.RegisteredObjects.IndexOf(CurrentDog.GetObject());
        UpdateValues();
    }


    public void SelectNext()
    {
        index = (index + 1) % Dogs.GetListSize();
        UpdateValues();
    }

    public void SelectPrev()
    {
        index = index - 1;
        if (index < 0)
        {
            index = Dogs.GetListSize() - 1;
        }

        UpdateValues();
    }
    private void UpdateValues()
    {
        CurrentDog.SetObjects(Dogs.GetItemAtIndex(index));

        DogImage.sprite = CurrentDog.GetObject().GetComponent<DogMovement>().Stats.MenuSprite;
    }
}
