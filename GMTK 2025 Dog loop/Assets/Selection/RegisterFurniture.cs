using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterFurniture : MonoBehaviour
{

    [SerializeField]
    public Furniture furniture;

    [SerializeField]
    FurnitureStore store;

    private void Awake()
    {
        furniture.furniture = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        store.GetObject().furniture.GetComponent<SpriteRenderer>().color = Color.red;

        store.Blocked = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        store.GetObject().furniture.GetComponent<SpriteRenderer>().color = Color.white;

        store.Blocked = false;
    }

}
