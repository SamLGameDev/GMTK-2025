using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Events/GameEventVector3")]
public class GameEventVector3 : ScriptableObject
{
    private List<GameEventListenerVector3> listeners = new List<GameEventListenerVector3>();
    public void Raise(Vector3 InVec)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(InVec);
        }

    }
    public void RegisterListener(GameEventListenerVector3 listener)
    {
        listeners.Add(listener);
    }
    public void UnRegisterListener(GameEventListenerVector3 listener)
    {
        listeners.Remove(listener);
    }
    public void UnRegisterAllListeners()
    {
        listeners.Clear();
    }
}