using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Powerup
{
    public string name;  
    [SerializeField] UnityEvent startAction;
    [SerializeField] UnityEvent endAction;

    // starts the powerup effect
    public void Start() => startAction.Invoke();

    // stops the powerup effect
    public void Stop() => endAction.Invoke();
}
