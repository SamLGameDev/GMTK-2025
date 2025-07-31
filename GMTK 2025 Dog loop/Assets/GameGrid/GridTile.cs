using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
    
    private Color NormalColor;

    [SerializeField]
    Color HighlightedColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = HighlightedColor; 
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = NormalColor;
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
