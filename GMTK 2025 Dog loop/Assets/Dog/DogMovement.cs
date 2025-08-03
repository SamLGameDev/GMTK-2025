using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DogMovement : MonoBehaviour
{
    Vector2 target;

    [SerializeField]
    float Speed;

    [SerializeField]
    float NewTargetTime;

    [SerializeField]
    Sprite Destroyedsprite;

    [SerializeField]
    GameGrid gameGrid;

    [SerializeField]
    Image DestructionBar;

    [SerializeField]
    int NumObjectsToLose;

    int CurrentDestroyedObjects;

    [SerializeField]
    float fillSpeed;

    [SerializeField]
    GameObject pawPrint;

    List<GameObject> pawPrints = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        pawPrints[0] = null;
        PickTarget();
        CurrentDestroyedObjects = NumObjectsToLose;
    }

    private void PickTarget()
    {
        GetRandomTarget();
        Invoke("PickTarget", NewTargetTime);
    }

    private void GetRandomTarget()
    {
        int random = Random.Range(0, 4);
        if (random == 0)
        {
            SetTarget();
        }
        else
        {
            int x = (int)Random.Range(0, gameGrid.gridSize.x);
            int y = (int)Random.Range(0, gameGrid.gridSize.y);
            target = gameGrid.GetTile(x, y).transform.position;
        }
    }

    private void SetTarget()
    {
        RegisterFurniture[] objects = FindObjectsByType<RegisterFurniture>((FindObjectsSortMode.None));

        target = objects[Random.Range(0, objects.Length)].gameObject.transform.position;
        Debug.Log("atempt");

        if (pawPrints[0] != null)
        {
            Debug.Log("place pawprint");
            GameObject print = pawPrints[pawPrints.Count - 1];
            print.transform.position = this.transform.position;
            print.SetActive(true);
            pawPrints.RemoveAt(pawPrints.Count - 1);
        }
        else
            CreatePawPrints();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        if (transform.position == (Vector3)target) 
        {
            GetRandomTarget();
        }

        float CurrentFillAmount = Mathf.MoveTowards(DestructionBar.fillAmount, (float)CurrentDestroyedObjects / NumObjectsToLose, fillSpeed * Time.deltaTime);

        DestructionBar.fillAmount = CurrentFillAmount;

        if (DestructionBar.fillAmount <= 0) 
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Untagged") 
        {
            RegisterFurniture furn = collision.GetComponent<RegisterFurniture>();

            furn.SetDestroyed();

            Destroy(furn);

            CurrentDestroyedObjects--;

            if (collision.gameObject == gameGrid.selectedObject.GetObject())
            {
                gameGrid.DropObject(gameObject);
            }
            
        }
    }

    private void CreatePawPrints()
    {
        for (int i = 0; i < 11; i++)
        {
            GameObject paw = Instantiate(Instantiate(pawPrint, this.transform.position, Quaternion.identity));
            pawPrints.Add(paw);
            paw.SetActive(false);
        }
    }
}
