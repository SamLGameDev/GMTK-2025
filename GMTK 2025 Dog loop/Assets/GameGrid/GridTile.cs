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
    FurnitureStore selectedTiles;

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
        if (selectedTiles.GetObject() != null) 
        {
            selectedTiles.GetObject().furniture.transform.position = transform.position;
        }

        GetComponent<SpriteRenderer>().color = HighlightedColor; 
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
