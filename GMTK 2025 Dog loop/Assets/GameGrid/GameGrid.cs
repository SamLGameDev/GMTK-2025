using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameGrid : MonoBehaviour
{

    [SerializeField]
    private Vector2 gridSize;

    [SerializeField]
    private float cellSize;

    [SerializeField]
    private GridTile tile;

    [SerializeField]
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        DrawGrid();
    }

    void DrawGrid() 
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GridTile tileInstance = Instantiate(tile, new Vector3(x, y), Quaternion.identity);
                tileInstance.name = $"Tile {x} {y}";

                if ((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0)) 
                {
                    tileInstance.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }

        cam.transform.position = new Vector3(gridSize.x / 2 - 0.5f, gridSize.y / 2 - 0.5f, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
