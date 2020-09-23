
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer : MonoBehaviour
{
    // Notify
    public abstract void OnNotify(string value);
}

public abstract class Subject : MonoBehaviour
{
    private List<Observer> _observers = new List<Observer>();

    //Register all observers
    public void RegisterObserver(Observer observer)
    {
        _observers.Add(observer);
    }
    
    //Notify
    public void Notify(string value)
    {
        foreach (var observer in _observers)
        {
            observer.OnNotify(value);
        }
    }
}
