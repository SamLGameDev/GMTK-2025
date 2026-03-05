using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dog/DogStats", order = 1)]
public class DogStats : ScriptableObject
{
    public float Speed;
    public float EnergyLevel;
    public float NewTargetTime;
    public float CountDownTime;
    public float AnimationSpeed;
    [Range(0, 1)]
    public float DogTargetCorrectness;

    public float DogEnrageTime;


    public float DogEnrageSpeeedGradualIncrease;

    public Sprite MenuSprite;

}
