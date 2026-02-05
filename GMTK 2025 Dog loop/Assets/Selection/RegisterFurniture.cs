using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class RegisterFurniture : MonoBehaviour
{

    [SerializeField]
    public Furniture furniture;

    [SerializeField]
    GameObjectStore store;

    [SerializeField]
    Sprite DestroyedSprite;

    /*[SerializeField]
    AudioSource furnitureDestroyedSFX;*/

    [SerializeField]
    ParticleSystem rubbleVFX;

    [SerializeField]
    FurnitureSet currentFurniture;

    [SerializeField]
    TextMeshProUGUI scoreDisplay;

    private BoxCollider2D collider;

    private void Awake()
    {
        furniture.furniture = gameObject;
        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        currentFurniture.Add(this);
    }

    public void SetDestroyed() 
    {
        GetComponent<SpriteRenderer>().sprite = DestroyedSprite;
        AudioSource furnitureDestroyedSFX = GameObject.FindGameObjectWithTag("DestroySFX").GetComponent<AudioSource>();
        if (furnitureDestroyedSFX != null )
            furnitureDestroyedSFX.Play();
        Instantiate(rubbleVFX, this.transform.position, Quaternion.identity);
    }

    private void Start()
    {
        collider.enabled = true;

        Vector3 scorePos = scoreDisplay.transform.parent.position;
        scorePos.x += this.collider.size.x / 2;
        scorePos.x -= scoreDisplay.transform.localScale.x / 2;
        scorePos.y += this.collider.size.y + 0.5f;
        scoreDisplay.transform.parent.position = scorePos;
    }

    private void OnDestroy()
    {
        currentFurniture.Remove(this);
    }

}
