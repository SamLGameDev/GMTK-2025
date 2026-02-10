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

    List<GameObject> pawPrints = new List<GameObject>(11);

    [SerializeField]
    Animator cameraShake;


    [SerializeField]
    GameObjectStore selectedObject;

    [SerializeField]
    GameObjectStore DogRegister;


    public bool countDown = true;

    public int CountdownTime;

    Animator dogAnimator;

    [SerializeField]
    GameObjectStore GameGrid;

    [SerializeField]
    AnimatorStore cameraAnimator;

    [SerializeField]
    int animationSpeed = 5;

    private bool stopDog = false;

    [SerializeField] private SelectedObjectMoverStore Mover;
    private void Awake()
    {
        dogAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

        gameGrid = GameGrid.GetObject().GetComponent<GameGrid>();
        DogRegister.SetObjects(gameObject);
        CountdownOver();
        Debug.Log("Dog start called");
        stopDog = false;
    }


    private void CountdownOver()
    {
        CreatePawPrints();
        PickTarget();
        CurrentDestroyedObjects = NumObjectsToLose;
        countDown = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void PickTarget()
    {
        if (stopDog)
            return;

        GetRandomTarget();
        Invoke("PickTarget", NewTargetTime);
    }

    private void GetRandomTarget()
    {
        int random = Random.Range(0, 3);
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
        if (countDown) 
        {
            return;
        }

        if (stopDog)
            return;

        transform.position = Vector2.MoveTowards(transform.position, target, Speed * Time.deltaTime);
        if (transform.position == (Vector3)target) 
        {
            GetRandomTarget();
        }




     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (stopDog)
            return;

        if (collision.CompareTag("Untagged")) 
        {
            RegisterFurniture furn = collision.GetComponent<RegisterFurniture>();

            if (!furn) return;

            furn.SetDestroyed();

            Destroy(furn);

            CurrentDestroyedObjects--;

            cameraAnimator.GetObject().Play("CameraShake");

            if (collision.gameObject == gameGrid.selectedObject.GetObject())
            {
                Mover.GetObject().StopMovingObject();
            }
            
        }
    }

    private void OnDestroy()
    {
        DogRegister.SetObjects(null);
        if (selectedObject.GetObject())
        {
           Mover.GetObject().StopMovingObject();
        }
    }
    private void CreatePawPrints()
    {
        for (int i = 0; i < 11; i++)
        {
            GameObject paw = Instantiate(pawPrint, this.transform.position, Quaternion.identity);
            pawPrints.Add(paw);
            paw.SetActive(false);
        }
    }

    private void SlowAnimation()
    {
        animationSpeed--;
        dogAnimator.speed = animationSpeed;
    }

    public void StopDog()
    {
        stopDog = true;
    }
}
