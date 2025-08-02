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

    public bool Right;

    public bool Left;

    public bool Down;

    public bool Up;

    private void Awake()
    {
        furniture.furniture = gameObject;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void SetDestroyed() 
    {
        GetComponent<SpriteRenderer>().sprite = DestroyedSprite;
    }

    private void Start()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "RightWall") 
        {
            Right = true;
        }
        if (collision.gameObject.tag == "LeftWall")
        {
            Left = true;
        }
        if (collision.gameObject.tag == "UpWall")
        {
            Up = true;
        }
        if (collision.gameObject.tag == "DownWall")
        {
            Down = true;
        }

        if (store.GetObject() != gameObject)
        {
            return;
        }

        if (collision.tag == "Untagged")
        {
            store.GetObject().GetComponent<SpriteRenderer>().color = Color.red;

            store.Blocked = true;
        }



    }

    private void OnTriggerExit2D(Collider2D collision)
    {


        if (collision.gameObject.tag == "RightWall")
        {
            Right = false;
        }
        if (collision.gameObject.tag == "LeftWall")
        {
            Left = false;
        }
        if (collision.gameObject.tag == "UpWall")
        {
            Up = false;
        }
        if (collision.gameObject.tag == "DownWall")
        {
            Down = false;
        }

        if (store.GetObject() != gameObject)
        {
            return;
        }

        if (collision.tag == "Untagged") 
        {
            store.GetObject().GetComponent<SpriteRenderer>().color = Color.white;

            store.Blocked = false;
        }


    }

}
