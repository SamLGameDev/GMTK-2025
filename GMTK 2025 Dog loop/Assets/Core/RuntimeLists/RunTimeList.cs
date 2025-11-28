using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class RunTimeList<T> : ScriptableObject
{

    protected readonly List<T> RegisteredObjects = new List<T>();

    public virtual void Add(T t)
    {

        if (!RegisteredObjects.Contains(t)) { RegisteredObjects.Add(t); }
    }
    public virtual void Remove(T t)
    {
        if (RegisteredObjects.Contains(t)) { RegisteredObjects.Remove(t); }
    }
    public T GetItemAtIndex(int index)
    {
        return RegisteredObjects[index];
    }
    public int GetIndexOfItem(T item)
    {
        for (int i = 0; i < RegisteredObjects.Count; i++)
        {
            if (RegisteredObjects[i].Equals(item))
            { return i; }
        }
        return -1;
    }
    public int GetListSize()
    {
        return RegisteredObjects.Count;
    }
    public List<T> GetItems()
    {
        return RegisteredObjects;
    }
}
