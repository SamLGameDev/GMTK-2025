using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterFurniture : MonoBehaviour
{

    [SerializeField]
    public Furniture furniture;

    [SerializeField]
    GameObjectStore store;

    [SerializeField]
    Sprite DestroyedSprite;

    [SerializeField]
    AudioSource furnitureDestroyedSFX;

    [SerializeField]
    ParticleSystem rubbleVFX;

    [SerializeField]
    FurnitureSet currentFurniture;

    private void Awake()
    {
        furniture.furniture = gameObject;
        GetComponent<BoxCollider2D>().enabled = false;
        currentFurniture.Add(this);

    }

    public void SetDestroyed() 
    {
        GetComponent<SpriteRenderer>().sprite = DestroyedSprite;
        furnitureDestroyedSFX.Play();
        Instantiate(rubbleVFX, this.transform.position, Quaternion.identity);
    }

    private void Start()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnDestroy()
    {
        currentFurniture.Remove(this);
    }

}
