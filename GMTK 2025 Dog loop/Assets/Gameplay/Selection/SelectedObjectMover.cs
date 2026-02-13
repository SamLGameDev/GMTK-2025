using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameGrid;

public class SelectedObjectMover : MonoBehaviour
{

    [SerializeField] GameObjectStore store;

    [SerializeField] Camera cam;
     
    CancellationTokenSource movingObjectCTS = new CancellationTokenSource();

    [SerializeField] private SelectedObjectMoverStore selfRef;

    [SerializeField]
    InputController IC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfRef.SetObjects(this);
    }

    public void StartMovingObject()
    {
        UpdateSelectedObject(movingObjectCTS.Token);
    }


    public void StopMovingObject()
    {
        movingObjectCTS.Cancel();
        store.GetObject().layer = 0;
    }

    private async UniTaskVoid UpdateSelectedObject(CancellationToken Token)
    {
        if (!store.GetObject())
        {
            return;
        }
        Vector3 LastValidPos = store.GetObject().transform.position;
        Vector3 Offset = IC.TouchWorldPos - store.GetObject().transform.position;
        bool bWasInvalidPos = false;
        ISelectable selectable = store.GetObject().GetComponent<ISelectable>();
        BoxCollider2D boxCollider = store.GetObject().GetComponent<BoxCollider2D>();
        bool bHasHitWall = false;
        bool down = false;
        bool up = false;
        bool right = false;
        bool left = false;
        while (true)
        {
            if (Token.IsCancellationRequested || !store.GetObject())
            {
                movingObjectCTS = new CancellationTokenSource();
                store.GetObject().transform.position = LastValidPos;


                if (bWasInvalidPos)
                {
                    selectable.OnInvalidLastPos();
                }
                else if (bHasHitWall)
                {
                    if (down)
                        LastValidPos.y += 0.5f;
                    else if (up)
                        LastValidPos.y -= 0.5f;
                    else if (left)
                        LastValidPos.x += 0.5f;
                    else if (right)
                        LastValidPos.x -= 0.5f;

                    store.GetObject().transform.position = LastValidPos;
                    selectable.OnInvalidLastPos();
                }
                else
                {
                    selectable.OnDrop();
                }

                store.SetObjects(null);
                
                break;
            }

            await UniTask.Yield();


            RaycastHit2D[] hits;

            Debug.Log(store.GetObject());
            hits = Physics2D.BoxCastAll(store.GetObject().gameObject.transform.position, boxCollider.size, 0, Vector2.zero, 0, 7);

            Color color = Color.white;
            bool bBlocked = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (!hit.rigidbody)
                {

                    if (selectable.CanMoveInsideWalls()) continue;


                    if (hit.collider.gameObject.tag == "DownWall")
                    {
                        bHasHitWall = true;
                        down = true;
                        break;
                    }
                    if (hit.collider.gameObject.tag == "UpWall")
                    {
                        bHasHitWall = true;
                        up = true;
                        break;
                    }
                    if (hit.collider.gameObject.tag == "LeftWall")
                    {
                        bHasHitWall = true;
                        left = true;
                        break;
                    }
                    if (hit.collider.gameObject.tag == "RightWall")
                    {
                        bHasHitWall = true;
                        right = true;
                        break;
                    }

                    continue;
                }

                if (hit.rigidbody.gameObject == store.GetObject())
                {
                    continue;
                }

                bBlocked = true;
                color = selectable.GetInvalidColor();

            }


            if (!bHasHitWall || selectable.CanMoveInsideWalls())
            {
                if (!bBlocked)
                {
                    LastValidPos = IC.TouchWorldPos - Offset;
                    bWasInvalidPos = false;
                }
                else
                {
                    bWasInvalidPos = true;
                }

              //  print("TryingToMove" + IC.TouchWorldPos - Offset);

                store.GetObject().transform.position = IC.TouchWorldPos - Offset;
                store.GetObject().GetComponent<SpriteRenderer>().color = color;
                store.Blocked = bBlocked;
            }
            if (bHasHitWall)
            {
                Vector3 newPos = LastValidPos;

                if (down)
                    newPos.y = LastValidPos.y + 0.5f;
                else if (up)
                    newPos.y = LastValidPos.y - 0.5f;
                else if (left)
                    newPos.x = LastValidPos.x + 0.5f;
                else if (right)
                    newPos.x = LastValidPos.x - 0.5f;


                store.GetObject().transform.position = newPos;
                //bWasInvalidPos = true;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {


    }

}