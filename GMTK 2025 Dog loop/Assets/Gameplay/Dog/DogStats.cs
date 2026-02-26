using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dog/DogStats", order = 1)]
public class DogStats : ScriptableObject
{
    public float Speed;
    public float EnergyLevel;
    public float NewTargetTime;
    public float CountDownTime;
    public float AnimationSpeed;

    public Sprite MenuSprite;

}
