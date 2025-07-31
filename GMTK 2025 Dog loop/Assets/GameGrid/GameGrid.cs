using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Tilemaps;


[ExecuteInEditMode]
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


    [SerializeField]
    private Color NormalOddColor;


    [SerializeField]
    private Color NormalEvenColor;

    private Dictionary<Vector2, GridTile> grid;

    // Start is called before the first frame update
    void Start()
    {
    }

    [ContextMenu("DrawGrid")]
    void DrawGrid() 
    {

        if (grid != null && grid.Count > 0)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    GridTile tile = GetTile(x, y);
                    DestroyImmediate(tile.gameObject);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
                }
            }
        }

        grid = new Dictionary<Vector2, GridTile>();
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GridTile tileInstance = Instantiate(tile, new Vector3(x, y), Quaternion.identity);
                tileInstance.name = $"Tile {x} {y}";

                SpriteRenderer renderer = tileInstance.GetComponent<SpriteRenderer>();

                if ((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0)) 
                {
                    renderer.color = NormalEvenColor;
                }
                else 
                {
                    renderer.color = NormalOddColor;
                }

                tileInstance.SetNormalColor(renderer.color);

                grid[new Vector2(x, y)] = tileInstance;
            }
        }

        cam.transform.position = new Vector3(gridSize.x / 2 - 0.5f, gridSize.y / 2 - 0.5f, -10);
    }

    GridTile GetTile(int x, int y)
    {
        return grid[new Vector2(x, y)];
    }
    GridTile GetTile(Vector2 Pos)
    {
        return grid[Pos];
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
