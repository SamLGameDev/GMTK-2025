using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterFurniture : MonoBehaviour
{

    [SerializeField]
    public Furniture furniture;

    [SerializeField]
    GameObjectStore store;

    private void Awake()
    {
        furniture.furniture = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (store.GetObject() != gameObject) 
        {
            return;
        }

        if (collision.gameObject.tag == "RightWall") 
        {
            store.Right = true;
        }
        if (collision.gameObject.tag == "LeftWall")
        {
            store.Left = true;
        }
        if (collision.gameObject.tag == "UpWall")
        {
            store.Up = true;
        }
        if (collision.gameObject.tag == "DownWall")
        {
            store.Down = true;
        }


        if (collision.tag == "Untagged")
        {
            store.GetObject().GetComponent<SpriteRenderer>().color = Color.red;

            store.Blocked = true;
        }



    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (store.GetObject() != gameObject)
        {
            return;
        }

        if (collision.gameObject.tag == "RightWall")
        {
            store.Right = false;
        }
        if (collision.gameObject.tag == "LeftWall")
        {
            store.Left = false;
        }
        if (collision.gameObject.tag == "UpWall")
        {
            store.Up = false;
        }
        if (collision.gameObject.tag == "DownWall")
        {
            store.Down = false;
        }

        if (collision.tag == "Untagged") 
        {
            store.GetObject().GetComponent<SpriteRenderer>().color = Color.white;

            store.Blocked = false;
        }


    }

}
