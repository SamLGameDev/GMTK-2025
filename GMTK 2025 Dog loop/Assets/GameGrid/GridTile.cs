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

    private void OnMouseDown()
    {
        
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
