using UnityEngine;

public abstract class RuntimeStore<T> : ScriptableObject
{
    [SerializeField]
    private T Item;

    public bool Blocked;

    public T GetObject()
    {
        return Item;
    }

    public void SetObjects(T Object)
    {
        Item = Object;
    }
}
