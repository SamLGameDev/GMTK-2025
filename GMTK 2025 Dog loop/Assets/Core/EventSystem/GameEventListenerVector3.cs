using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerVector3 : MonoBehaviour
{
    public GameEventVector3 Event;
    public Vec3UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }
    private void OnDisable()
    {
        Event.UnRegisterListener(this);
    }
    public virtual void OnEventRaised(Vector3 InVec)
    {
        Response.Invoke(InVec);
    }
}