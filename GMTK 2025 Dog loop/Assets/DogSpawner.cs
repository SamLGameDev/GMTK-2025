using UnityEditor;
using UnityEngine;

public class DogSpawner : MonoBehaviour
{

    public int CountdownTime;

    [SerializeField]
    private GameObjectStore Dog;

    [SerializeField]
    private BoolStore CanUseAbilities;

    [SerializeField] private Sprite emptyBed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(SpawnDog), CountdownTime);
        CanUseAbilities.SetValue(false);
    }

    private void SpawnDog()
    {
        Instantiate(Dog.GetObject(), transform.position, Quaternion.identity);
        CanUseAbilities.SetValue(true);
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = emptyBed;
    }
}
