using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using Cysharp.Threading.Tasks.Triggers;
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

    private DogMovement dog;
    [SerializeField] private GameObjectStore dogStore;

    [SerializeField] private GameEvent DogEnraged;

    [SerializeField] private GameEvent DogStoppedEnrage;

    [SerializeField] private float MoveSpeed = 200;
    [SerializeField] private LayerMask mask;

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
        Rigidbody2D rb = store.GetObject().GetComponent<Rigidbody2D>();
        float TimeHeld = 0;
        bool bHasDogEnraged = false;

        while (true)
        {


            if (!dog && dogStore.GetObject())
            {
                dogStore.GetObject().TryGetComponent<DogMovement>(out dog);
                TimeHeld = 0;
            }
            else if (dogStore.GetObject() && selectable.CanTriggerEnrage())
            {
                if (TimeHeld > dog.Stats.DogEnrageTime && !bHasDogEnraged)
                {
                    DogEnraged.Raise();
                    bHasDogEnraged = true;
                }
            }

            if (Token.IsCancellationRequested || !store.GetObject())
            {
                movingObjectCTS = new CancellationTokenSource();
                store.GetObject().transform.position = LastValidPos;

                rb.linearVelocity = Vector2.zero;
                if (bWasInvalidPos)
                {
                    selectable.OnInvalidLastPos();
                }
                else
                {
                    selectable.OnDrop();
                }

                if (bHasDogEnraged)
                {
                    DogStoppedEnrage.Raise();
                }

                store.SetObjects(null);

                break;
            }

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            TimeHeld += Time.fixedDeltaTime;

            Collider2D[] hits;
            hits = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.size, 0, mask);

         

            Color color = Color.white;
            bool bBlocked = false;
            foreach (Collider2D hit in hits)
            {
                print(hit.transform.name);
                if (!hit.attachedRigidbody)
                {
                    if (hit.transform.gameObject.layer == 8)
                    {
                        print("inWal");
                        bBlocked = true;
                        color = selectable.GetInvalidColor();
                    }
                    continue;
                }

                if (hit.gameObject == store.GetObject())
                {
                    continue;
                }

                bBlocked = true;
                color = selectable.GetInvalidColor();

            }


            if (!bBlocked)
            {
                LastValidPos = store.GetObject().transform.position;
                bWasInvalidPos = false;
            }
            else
            {
                bWasInvalidPos = true;
            }

            Vector2 velocity = ((IC.TouchWorldPos - Offset) - store.GetObject().transform.position) *
                               (MoveSpeed * Time.fixedDeltaTime);
            rb.linearVelocity = velocity;
            //  print("TryingToMove" + IC.TouchWorldPos - Offset);

            //store.GetObject().transform.position = IC.TouchWorldPos - Offset;
            store.GetObject().GetComponent<SpriteRenderer>().color = color;
            store.Blocked = bBlocked;


        }
    }

    // Update is called once per frame
    void Update()
    {


    }

}