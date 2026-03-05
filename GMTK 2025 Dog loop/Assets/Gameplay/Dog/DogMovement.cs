using Cysharp.Threading.Tasks;
using Gameplay.Furniture;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DogMovement : MonoBehaviour
{
    Vector2 target;


    public DogStats Stats;

    [SerializeField]
    Sprite Destroyedsprite;

    GameGrid gameGrid;

    [SerializeField] private FurnitureSet CurrentFurns;

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

    Animator dogAnimator;

    [SerializeField]
    GameObjectStore GameGrid;

    [SerializeField]
    AnimatorStore cameraAnimator;

    private bool stopDog = false;

    [SerializeField] private SquekyToyInfo SqueakyInfo;

    [SerializeField] private SelectedObjectMoverStore Mover;

    CancellationTokenSource movingToObjectCTS = new CancellationTokenSource();

    delegate void FOnReachedObject();

    private FOnReachedObject OnReachedObject;

    [SerializeField]
     private BoolStore CanUseAbilities;

    CancellationTokenSource PickTargetCTS = new CancellationTokenSource();


    CancellationTokenSource ChaseSelectedCTS = new CancellationTokenSource();

    [SerializeField] private GameEvent OnSqeakyToyOver;

    [SerializeField] private AudioSource dogPlayingWithToy;

    [SerializeField] private GameEvent dogReachedTugRope;

    private float CurrentAnimationSpeed;

    private float GradualSpeed = 0;


    private void Awake()
    {
        dogAnimator = GetComponent<Animator>();
        DogRegister.SetObjects(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

        gameGrid = GameGrid.GetObject().GetComponent<GameGrid>();
        CountdownOver();
        OnReachedObject += GetRandomTarget;
        MoveTowardsObject(movingToObjectCTS.Token);
        CurrentAnimationSpeed = Stats.AnimationSpeed;

        Debug.Log("Dog start called");
        stopDog = false;
    }

    private void CountdownOver()
    {
        CreatePawPrints();
        PickTarget(PickTargetCTS.Token);
        CurrentDestroyedObjects = CurrentFurns.GetListSize();
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
            await UniTask.WaitForSeconds(Stats.NewTargetTime);
        }
    }

    private void GetRandomTarget()
    {
        float random = Random.Range(0f, 1f);
        if (random <= Stats.DogTargetCorrectness)
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


    public void SetTargetRope(Vector3 Target)
    {

        OnReachedObject -= GetRandomTarget;
        PickTargetCTS.Cancel();
        target = Target;
        OnReachedObject += StartTugRopeWait;
        CanUseAbilities.SetValue(false);
    }


    public void StartSqueakyToyWait()
    {
        dogPlayingWithToy.Play();
        dogAnimator.SetBool("isChewingToy", true);
        movingToObjectCTS.Cancel();
        SqueakToyWait();

    }

    public void StartTugRopeWait()
    {
        dogPlayingWithToy.Play();
        dogAnimator.SetBool("isChewingToy", true);
        movingToObjectCTS.Cancel();
        dogReachedTugRope.Raise();

    }

    public void EndTugRope()
    {
        OnReachedObject -= StartTugRopeWait;
        OnReachedObject += GetRandomTarget;

        PickTargetCTS = new CancellationTokenSource();

        PickTarget(PickTargetCTS.Token);

        movingToObjectCTS = new CancellationTokenSource();
        MoveTowardsObject(movingToObjectCTS.Token);

        CanUseAbilities.SetValue(true);

        dogAnimator.SetBool("isChewingToy", false);

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
                transform.position = Vector2.MoveTowards(transform.position, target, (Stats.Speed  + GradualSpeed)* Time.fixedDeltaTime);
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
        CurrentAnimationSpeed--;
        dogAnimator.speed = CurrentAnimationSpeed;
    }

    public void StopDog()
    {
        stopDog = true;
    }

    private async UniTask ChaseSelectedObject(CancellationToken Token)
    {
        while (true)
        {
            if (Token.IsCancellationRequested) return;


            target = selectedObject.GetObject().transform.position;

            GradualSpeed += Stats.DogEnrageSpeeedGradualIncrease * Time.fixedDeltaTime;

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);

        }

    }

    public void Enrage()
    {
        PickTargetCTS.Cancel();

        ChaseSelectedObject(ChaseSelectedCTS.Token);
    }

    public void StopEnrage()
    {
        ChaseSelectedCTS.Cancel();

        ChaseSelectedCTS = new CancellationTokenSource();

        PickTargetCTS = new CancellationTokenSource();

        PickTarget(PickTargetCTS.Token);

        GradualSpeed = 0;
    }


}
