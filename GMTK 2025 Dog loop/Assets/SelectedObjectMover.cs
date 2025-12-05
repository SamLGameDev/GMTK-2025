using UnityEngine;

public class SelectedObjectMover : MonoBehaviour
{

    [SerializeField]
    GameObjectStore store;

    [SerializeField]
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!store.GetObject()) return;

        RaycastHit2D[] hits;

        BoxCollider2D boxCollider = store.GetObject().GetComponent<BoxCollider2D>();

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -cam.transform.position.z;
        Vector3 Pos = cam.ScreenToWorldPoint(mousePos);
        Pos.z = 1;

        Vector2 DrawPos = Pos;
        DrawPos.y += boxCollider.size.y / 2;
        DrawPos.x += 0.5f * (boxCollider.size.x - 1);

        hits = Physics2D.BoxCastAll(DrawPos, boxCollider.size, 0, Vector2.zero, 0, 7);

        Color color = Color.white;
        bool bBlocked = false;

        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.rigidbody)
            {

                if (hit.collider.gameObject.tag == "Wall")
                {
                    // store.GetObject().GetComponent<SpriteRenderer>().color = Color.red;
                   // color = Color.red;
                    return;
                }
                continue;
            }

            if (hit.rigidbody.gameObject == store.GetObject())
            {
                continue;
            }
            bBlocked = true;
            color = Color.red;


        //    return;

        }
        store.GetObject().transform.position = Pos;
        store.GetObject().GetComponent<SpriteRenderer>().color = color;
        store.Blocked = bBlocked;
    }

}

