using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Tilemaps;
using NUnit.Framework;
using System;
using Unity.VisualScripting;


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


    [SerializeField]
    private List<PosTilePair> grid;

    [SerializeField]
    private FurnitureStore selectedObject;

    public delegate void OnClicked(Furniture tile);

    public OnClicked clicked;

    [Serializable]
    struct PosTilePair 
    {
        public Vector2 Pos;
        public GridTile Tile;
    }

    float TimeSinceLastClick = -0.5f;

    // Start is called before the first frame update
    void Start()
    {
        clicked = SetSelected;
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

        grid = new List<PosTilePair>();

        GameObject Parent = Instantiate(gameObject);

        Parent.name = "Grid";
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GridTile tileInstance = Instantiate(tile, new Vector3(x, y), Quaternion.identity, Parent.transform);
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

                PosTilePair pair =new PosTilePair();
                pair.Tile = tileInstance;
                pair.Pos = new Vector2(x, y);

                grid.Add(pair);

                tileInstance.gamegrid = this;
            }
        }

        cam.transform.position = new Vector3(gridSize.x / 2 - 0.5f, gridSize.y / 2 - 0.5f, -10);
    }

    public GridTile GetTile(int x, int y)
    {
        foreach (PosTilePair posTilePair in grid) 
        {
            if (posTilePair.Pos == new Vector2(x, y))
            {
                return posTilePair.Tile;
            }
        }
        return null;
    }
    public GridTile GetTile(Vector2 Pos)
    {
        foreach (PosTilePair posTilePair in grid)
        {
            if (posTilePair.Pos == Pos)
            {
                return posTilePair.Tile;
            }
        }
        return null;
    }


    public void SetSelected(Furniture tile)
    {
        selectedObject.SetObjects(tile);

        tile.furniture.gameObject.layer = 2;
        clicked = DropObject;
    }



    public void DropObject(Furniture tile)
    {
        if (selectedObject.Blocked || !selectedObject.GetObject())
        {
            return;
        }

        print("why");


        selectedObject.GetObject().furniture.layer = 0;

        selectedObject.SetObjects(null);

        clicked = SetSelected;

    }



    private void OnDestroy()
    {
        selectedObject.SetObjects(null);
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) &&  TimeSinceLastClick + 0.5f < Time.time)
        {
            if (selectedObject.GetObject() != null)
            {
                clicked.Invoke(selectedObject.GetObject());
                return;
            }

            TimeSinceLastClick = Time.time;
            Vector3 Pos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(Pos, new Vector2(0, 0));

            if (hit)
            {

                if (!hit.rigidbody)
                {
                    return;
                }

                GameObject furn = hit.rigidbody.gameObject;



                RegisterFurniture store = furn.GetComponent<RegisterFurniture>();

                clicked.Invoke(store.furniture);
            }

        }
        
    }
}

// Could change system to instead check for raycast if hit, but then use grid position for changing location, eliminates need for tiles to kknow about furniture
//composite colider