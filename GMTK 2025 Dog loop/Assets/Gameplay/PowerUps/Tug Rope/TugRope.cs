using UnityEngine;

public class TugRope : MonoBehaviour, ISelectable
{
    [SerializeField] private GameEventVector3 TugRopeDropped;

    [SerializeField] private GameObjectSet PowerupPool;

    [SerializeField] private Color InvalidColor;

    [SerializeField] private AudioSource TugRopeDroppedNoise;
    public void OnDrop()
    { 
        TugRopeDroppedNoise.Play();
        TugRopeDropped.Raise(transform.position);
    }

    public void OnInvalidLastPos()
    {
        PowerupPool.Add(gameObject);
        gameObject.SetActive(false);
    }

    public bool CanMoveInsideWalls()
    {
        return true;
    }

    public Color GetInvalidColor()
    {
        return InvalidColor;
    }

    public void Remove()
    {
        if (gameObject.activeSelf)
        {
            Destroy(gameObject);
        }
    }
}
