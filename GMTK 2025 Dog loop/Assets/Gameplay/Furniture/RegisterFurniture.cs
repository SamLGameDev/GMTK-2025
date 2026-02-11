using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay.Furniture
{
    public class RegisterFurniture : MonoBehaviour, ISelectable
    {

        [SerializeField]
        public global::Furniture furniture;

        [SerializeField]
        GameObjectStore store;

        [SerializeField]
        Sprite DestroyedSprite;

        [SerializeField]
        AudioSource furnitureDestroyedSFX;

        [SerializeField]
        ParticleSystem rubbleVFX;

        [SerializeField]
        FurnitureSet currentFurniture;

        [SerializeField]
        AudioSource gridSnapSFX;

        [SerializeField]
        AudioSource pointsIncreaseSFX;

        public bool bShouldDestroy = false;

        private BoxCollider2D collider;

        [SerializeField] private TextMeshProUGUI scoreDisplay;

        private float scoreCountDown;

        private void Awake()
        {
            furniture.furniture = gameObject;
            collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;
            currentFurniture.Add(this);

        }

        public void SetDestroyed() 
        {
            GetComponent<SpriteRenderer>().sprite = DestroyedSprite;
            //GetComponent<SpriteRenderer>().color = Color.grey;
            furnitureDestroyedSFX.Play();
            Instantiate(rubbleVFX, this.transform.position, Quaternion.identity);
            bShouldDestroy = true;
            Destroy(this);
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

        private void OnDestroy()
        {
            currentFurniture.Remove(this);
        }

        public void OnDrop()
        {
            gridSnapSFX.Play();

            //if (this == null) return;

            print("why");

            if (!bShouldDestroy)
            {
                SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                if (sprite.color == Color.red)
                {
                    sprite.color = Color.white;
                }
            }
        }

        public void OnInvalidLastPos()
        {
            OnDrop();
        }

        public bool CanMoveInsideWalls()
        {
            return false;
        }

        public Color GetInvalidColor()
        {
            return Color.red;
        }

        public void SetCountDown()
        {
            scoreCountDown = furniture.GetScore();
            scoreDisplay.gameObject.SetActive(true);
            pointsIncreaseSFX.Play();
        }

        public bool GiveScoreToLeader()
        {
            scoreCountDown -= 1;
            scoreDisplay.text = scoreCountDown.ToString();

            if (scoreCountDown <=0)
                return false;
            else
                return true;
        }

    }
}
