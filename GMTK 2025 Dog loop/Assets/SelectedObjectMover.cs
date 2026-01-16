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
    AudioSource gridSnapSFX;

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
        gridSnapSFX.Play();
    }

    private async UniTaskVoid UpdateSelectedObject(CancellationToken Token)
    {
        if (!store.GetObject())
        {
            return;
        }

        Vector3 LastValidPos = store.GetObject().transform.position;
        Vector3 Offset = IC.TouchWorldPos - store.GetObject().transform.position;
        while (true)
        {
            if (Token.IsCancellationRequested || !store.GetObject())
            {
                movingObjectCTS = new CancellationTokenSource();
                store.GetObject().transform.position = LastValidPos;
                store.GetObject().GetComponent<SpriteRenderer>().color = Color.white;
                store.SetObjects(null);
                
                break;
            }

            await UniTask.Yield();


            RaycastHit2D[] hits;

            BoxCollider2D boxCollider = store.GetObject().GetComponent<BoxCollider2D>();


            Vector2 DrawPos = IC.TouchWorldPos - Offset;
            DrawPos.y += boxCollider.size.y / 2;
            DrawPos.x += 0.5f * (boxCollider.size.x - 1);

            hits = Physics2D.BoxCastAll(DrawPos, boxCollider.size, 0, Vector2.zero, 0, 7);

            Color color = Color.white;
            bool bBlocked = false;
            bool bHasHitWall = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (!hit.rigidbody)
                {

                    if (hit.collider.gameObject.CompareTag("Wall"))
                    {
                        bHasHitWall = true;
                        break;
                    }

                    continue;
                }

                if (hit.rigidbody.gameObject == store.GetObject())
                {
                    continue;
                }

                bBlocked = true;
                color = Color.red;

            }

            if (!bHasHitWall)
            {


                if (!bBlocked)
                {
                    LastValidPos = IC.TouchWorldPos - Offset;
                }

                store.GetObject().transform.position = IC.TouchWorldPos - Offset;
                store.GetObject().GetComponent<SpriteRenderer>().color = color;
                store.Blocked = bBlocked;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {


    }

}