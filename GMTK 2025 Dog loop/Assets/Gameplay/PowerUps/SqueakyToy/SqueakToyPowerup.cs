using UnityEngine;

public class SqueakToyPowerup : MonoBehaviour, ISelectable
{
    [SerializeField] private GameEventVector3 SquekyToyDropped;

    [SerializeField] private GameObjectSet PowerupPool;

    [SerializeField] private Color InvalidColor;

    [SerializeField] private AudioSource squeakerDropped;

    [SerializeField] private GameEvent OnSqueakyToyInvalidDrop;
        
    public void OnDrop()
    {
        squeakerDropped.Play();
        SquekyToyDropped.Raise(transform.position);
    }

    public void OnInvalidLastPos()
    {
       PowerupPool.Add(gameObject);
       gameObject.SetActive(false);
       OnSqueakyToyInvalidDrop.Raise();

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

    public bool CanTriggerEnrage() 
    {
        return false;
    }

}
