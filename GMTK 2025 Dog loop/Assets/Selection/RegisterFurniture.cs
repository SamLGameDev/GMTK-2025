using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;

public class RegisterFurniture : MonoBehaviour
{

    [SerializeField]
    public Furniture furniture;

    [SerializeField]
    GameObjectStore store;

    [SerializeField]
    Sprite DestroyedSprite;

    /*[SerializeField]
    AudioSource furnitureDestroyedSFX;*/

    [SerializeField]
    ParticleSystem rubbleVFX;

    [SerializeField]
    FurnitureSet currentFurniture;

    [SerializeField]
    TextMeshProUGUI scoreDisplay;

    private BoxCollider2D collider;

    private SpriteRenderer spriteRenderer;

    private float scoreCountDown;
    private bool triggerDisplay = false;

    private void Awake()
    {
        furniture.furniture = gameObject;
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider.enabled = false;
        currentFurniture.Add(this);
    }

    public void SetDestroyed() 
    {
        spriteRenderer.sprite = DestroyedSprite;
        spriteRenderer.color = Color.grey;
        AudioSource furnitureDestroyedSFX = GameObject.FindGameObjectWithTag("DestroySFX").GetComponent<AudioSource>();
        if (furnitureDestroyedSFX != null )
            furnitureDestroyedSFX.Play();
        Instantiate(rubbleVFX, this.transform.position, Quaternion.identity);
    }

    private void Start()
    {
        collider.enabled = true;

        Vector3 scorePos = scoreDisplay.transform.parent.position;
        scorePos.x += this.collider.size.x / 2;
        scorePos.x -= scoreDisplay.transform.localScale.x / 2;
        scorePos.y += this.collider.size.y + 0.5f;
        scoreDisplay.transform.parent.position = scorePos;
    }

    private void Update()
    {
    }

    private void OnDestroy()
    {
        currentFurniture.Remove(this);
    }

    public async UniTask TriggerScoreDisplay()
    {
        while (true)
        {
            if (!triggerDisplay)
            {
                triggerDisplay = true;
                scoreCountDown = furniture.GetScore();
                scoreDisplay.gameObject.SetActive(true);
            }

            scoreCountDown -= 1;
            scoreDisplay.text = scoreCountDown.ToString();

            if (scoreCountDown == 0)
            {
                triggerDisplay = false;
                CancelInvoke();
                break;
            }
            await UniTask.WaitForSeconds(0.075f);
        }

    }
}
