using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GridTile : MonoBehaviour
{

    [ReadOnly(true), SerializeField]
    private Color NormalColor;

    [SerializeField]
    Color HighlightedColor;

    [SerializeField]
    GameObjectStore selectedTiles;

    [SerializeField]
    Sprite EmptyTile;

    public Furniture Furniture;

    delegate void OnClicked();

    public GameGrid gamegrid;


    // Start is called before the first frame update
    void Start()
    {
        EmptyTile = GetComponent<SpriteRenderer>().sprite;
        if (Furniture != null) 
        {
            Furniture.furniture.transform.position = transform.position;
        }
    }

    private void OnMouseEnter()
    {

        GetComponent<SpriteRenderer>().color = HighlightedColor;

        if (selectedTiles.GetObject() != null) 
        {
            if (Vector2.Dot(selectedTiles.GetObject().transform.position - transform.position, Vector3.right) < 0 && selectedTiles.GetObject().GetComponent<RegisterFurniture>().Right)
            {
                return;
            }
            if (Vector2.Dot(selectedTiles.GetObject().transform.position - transform.position, Vector3.up) < 0 && selectedTiles.GetObject().GetComponent<RegisterFurniture>().Up)
            {
                return;
            }
            if (Vector2.Dot(selectedTiles.GetObject().transform.position - transform.position, Vector3.down) < 0 && selectedTiles.GetObject().GetComponent<RegisterFurniture>().Down)
            {
                return;
            }
            if (Vector2.Dot(selectedTiles.GetObject().transform.position - transform.position, Vector3.left) < 0 && selectedTiles.GetObject().GetComponent<RegisterFurniture>().Left)
            {
                return;
            }
            selectedTiles.GetObject().transform.position = transform.position;
        }

    }

    private void OnMouseExit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = NormalColor;

        spriteRenderer.sprite = EmptyTile;

    }


    public void SetNormalColor(Color color) 
    {
        NormalColor = color;
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
