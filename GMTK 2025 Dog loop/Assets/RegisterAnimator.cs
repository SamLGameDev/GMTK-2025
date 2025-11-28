using UnityEngine;

public class RegisterAnimator : MonoBehaviour
{

    [SerializeField]
    AnimatorStore store;

    [SerializeField]
    Animator Animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        store.SetObjects(Animator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
