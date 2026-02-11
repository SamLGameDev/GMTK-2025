using Cysharp.Threading.Tasks;
using Gameplay.Furniture;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    [SerializeField] private SquekyToyInfo SqueakyInfo;

    [SerializeField] private SelectedObjectMoverStore Mover;

    CancellationTokenSource movingToObjectCTS = new CancellationTokenSource();

    delegate void FOnReachedObject();

    private FOnReachedObject OnReachedObject;

    [SerializeField]
     private BoolStore CanUseAbilities;

    CancellationTokenSource PickTargetCTS = new CancellationTokenSource();

    [SerializeField] private GameEvent OnSqeakyToyOver;

    [SerializeField] private AudioSource dogPlayingWithToy;

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
        OnReachedObject += GetRandomTarget;
        MoveTowardsObject(movingToObjectCTS.Token);

        Debug.Log("Dog start called");
        stopDog = false;
    }

    private void CountdownOver()
    {
        CreatePawPrints();
        PickTarget(PickTargetCTS.Token);
        CurrentDestroyedObjects = NumObjectsToLose;
        countDown = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private async UniTask PickTarget(CancellationToken Token)
    {
        while (true)
        {
            if (Token.IsCancellationRequested)
            {
                break;
            }

            GetRandomTarget();
            await UniTask.WaitForSeconds(NewTargetTime);
        }
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

        if (pawPrints[0] != null)
        {
            GameObject print = pawPrints[pawPrints.Count - 1];
            print.transform.position = this.transform.position;
            print.SetActive(true);
            pawPrints.RemoveAt(pawPrints.Count - 1);
        }
        else
            CreatePawPrints();
    }

    public void SetTarget(Vector3 Target)
    {

        OnReachedObject -= GetRandomTarget;
        PickTargetCTS.Cancel();
        target = Target;
        OnReachedObject += StartSqueakyToyWait;
        CanUseAbilities.SetValue(false);
    }

    public void StartSqueakyToyWait()
    {
        dogPlayingWithToy.Play();
        dogAnimator.SetBool("isChewingToy", true);
        movingToObjectCTS.Cancel();
        SqueakToyWait();

    }

    public async UniTask SqueakToyWait()
    {
        await UniTask.WaitForSeconds(SqueakyInfo.Duration);
        OnReachedObject -= StartSqueakyToyWait;
        OnReachedObject += GetRandomTarget;

        OnSqeakyToyOver.Raise();

        PickTargetCTS = new CancellationTokenSource();

        PickTarget(PickTargetCTS.Token);

        movingToObjectCTS = new CancellationTokenSource();
        MoveTowardsObject(movingToObjectCTS.Token);

        CanUseAbilities.SetValue(true);

        dogAnimator.SetBool("isChewingToy", false);

    }


    public async UniTask MoveTowardsObject(CancellationToken Token)
    {
        while (true)
        {
            if (stopDog)
            {
                continue;
            }

            if (target != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, Speed * Time.fixedDeltaTime);
                if (transform.position.Equals(target))
                {
                    OnReachedObject.Invoke();
                }
            }

            if (Token.IsCancellationRequested)
            {
                break;
            }

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);

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

            furn.bShouldDestroy = true;

            if (collision.gameObject == gameGrid.selectedObject.GetObject())
            {
                Mover.GetObject().StopMovingObject();
            }

            furn.SetDestroyed();

            CurrentDestroyedObjects--;

            cameraAnimator.GetObject().Play("CameraShake");
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
