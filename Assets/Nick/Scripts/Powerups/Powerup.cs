using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Powerup
{
    public string name;  
    [SerializeField] UnityEvent startAction;
    [SerializeField] UnityEvent endAction;

    public void Start() => startAction.Invoke();
    public void Stop() => endAction.Invoke();
}
