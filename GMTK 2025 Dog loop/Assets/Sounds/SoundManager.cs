using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip Bark;

    [SerializeField]
    AudioSource Barking;

    [SerializeField]
    float BarkingMinDelay;

    [SerializeField]
    float BarkingMaxDelay;

    // Start is called before the first frame update
    void Start()
    {
        PlayBarkSound();
    }

    private void PlayBarkSound()
    {
        Barking.PlayOneShot(Bark);
        float Delay = Random.Range(BarkingMinDelay, BarkingMaxDelay);
        print(Delay);
        Invoke("PlayBarkSound", Delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
