using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{


    [SerializeField]
    private Camera Cam;

    [SerializeField]
    GameObjectStore SelectedObject;

    [SerializeField]
    SelectedObjectMover Mover;

    public Vector3 TouchWorldPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    public void UpdateTouchPosition(InputAction.CallbackContext context)
    {
        TouchWorldPos = Cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
        TouchWorldPos.z = 1;
    }

    private void OnDragStart()
    { 
        RaycastHit2D[] hits = Physics2D.CircleCastAll(TouchWorldPos, 0.2f, Vector2.zero, 0, 7);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit)
            {
                ISelectable selectable;
                if (!hit.rigidbody || !hit.rigidbody.TryGetComponent<ISelectable>(out selectable))
                {
                    return;
                }

                GameObject furn = hit.rigidbody.gameObject;
                
                SelectedObject.SetObjects(furn);

                furn.layer = 2;

                Mover.StartMovingObject();
            }
        }
    }


    public void Interact(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Invoke(nameof(OnDragStart), 0);
        }
        else if (context.canceled && SelectedObject.GetObject())
        {
            Mover.StopMovingObject();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
