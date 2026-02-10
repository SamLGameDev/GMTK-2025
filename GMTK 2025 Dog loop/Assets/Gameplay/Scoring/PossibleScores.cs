using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Scoring/PossibleScores", order = 1)]
public class PossibleScores : ScriptableObject
{
    public enum Scores
    {
        High,
        Mid,
        Low
    }
    [SerializeField]
    private float HighScore;
    [SerializeField]
    private float MidScore;
    [SerializeField]
    private float LowScore;

    public float GetScore(Scores Score)
    {
        switch (Score)
        {
            case (Scores.High):
            {
                return HighScore;
            }
            case (Scores.Mid):
            {
                return MidScore;
            }
            case (Scores.Low):
            {
                return LowScore;
            }
            default:
            {
                return 0;
            }
        }
    }
    
}
