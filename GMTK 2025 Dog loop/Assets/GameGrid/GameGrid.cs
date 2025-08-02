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
    private GameObjectStore selectedObject;

    public delegate void OnClicked(GameObject tile);

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

                    if (tile == null) 
                    {
                        continue;
                    }
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


    public void SetSelected(GameObject furn)
    {
        selectedObject.SetObjects(furn);

        furn.layer = 2;
        clicked = DropObject;

        print("Select");
    }



    public void DropObject(GameObject furn)
    {
        if (selectedObject.Blocked || !selectedObject.GetObject())
        {
            return;
        }

        print("Drop");


        selectedObject.GetObject().layer = 0;

        selectedObject.SetObjects(null);

        clicked = SetSelected;

        selectedObject.Left = false;
        selectedObject.Right = false;
        selectedObject.Up = false;
        selectedObject.Down = false;

    }

    public void Interact()
    {
        if (!selectedObject) 
        {
            print("WTF");
            return;
        }

        if (selectedObject.GetObject())
        {
            clicked.Invoke(selectedObject.GetObject());
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -cam.transform.position.z;
        Vector3 Pos = cam.ScreenToWorldPoint(mousePos);
        Pos.z = 1;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(Pos, 0.2f, Vector2.zero, 0, 7);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit)
            {

                if (!hit.rigidbody)
                {


                    print(hit.collider.gameObject.name);
                    return;
                }

                GameObject furn = hit.rigidbody.gameObject;


                print("hit");


                clicked.Invoke(furn);
            }
        }
    }

    private void OnDestroy()
    {
        selectedObject.SetObjects(null);
    }
    // Update is called once per frame
    void Update()
    {
    }
}

// Could change system to instead check for raycast if hit, but then use grid position for changing location, eliminates need for tiles to kknow about furniture
//composite colider