using UnityEngine;

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

        private bool bShouldDestroy = false;

        private void Awake()
        {
            furniture.furniture = gameObject;
            GetComponent<BoxCollider2D>().enabled = false;
            currentFurniture.Add(this);

        }

        public void SetDestroyed() 
        {
            GetComponent<SpriteRenderer>().sprite = DestroyedSprite;
            GetComponent<SpriteRenderer>().color = Color.grey;
            furnitureDestroyedSFX.Play();
            Instantiate(rubbleVFX, this.transform.position, Quaternion.identity);
            bShouldDestroy = true;
        }

        private void Start()
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }

        private void OnDestroy()
        {
            currentFurniture.Remove(this);
        }

        public void OnDrop()
        {
            gridSnapSFX.Play();
            print("why");
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            if (sprite.color == Color.red)
            {
                sprite.color = Color.white;
            }
            if (bShouldDestroy)
            {
                Destroy(this);
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

    }
}
