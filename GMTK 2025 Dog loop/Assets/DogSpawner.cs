using UnityEditor;
using UnityEngine;

public class DogSpawner : MonoBehaviour
{

    public int CountdownTime;

    [SerializeField]
    private GameObject Dog;

    [SerializeField]
    private BoolStore CanUseAbilities;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(SpawnDog), CountdownTime);
        CanUseAbilities.SetValue(false);
    }

    private void SpawnDog()
    {
        Instantiate(Dog, transform.position, Quaternion.identity);
        CanUseAbilities.SetValue(true);
    }
}
