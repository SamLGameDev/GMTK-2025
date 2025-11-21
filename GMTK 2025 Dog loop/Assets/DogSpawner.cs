using UnityEditor;
using UnityEngine;

public class DogSpawner : MonoBehaviour
{

    public int CountdownTime;

    [SerializeField]
    private GameObject Dog;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(SpawnDog), CountdownTime);
    }

    private void SpawnDog()
    {
        Instantiate(Dog, transform.position, Quaternion.identity);
    }
}
