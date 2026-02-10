using UnityEngine;

public class SqueakToyPowerup : MonoBehaviour, ISelectable
{
    [SerializeField] private GameEventVector3 SquekyToyDropped;

    [SerializeField] private GameObjectSet PowerupPool;

    [SerializeField] private Color InvalidColor;

    [SerializeField] private AudioSource squeakerDropped;
        
    public void OnDrop()
    {
        squeakerDropped.Play();
        SquekyToyDropped.Raise(transform.position);
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
