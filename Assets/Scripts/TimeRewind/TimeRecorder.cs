using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public class TimeRecorder : MonoBehaviour
{
    public int maxStates = 600;

    private Rigidbody2D _rigidbody2D;
    private FieldInfo[] _fieldInfo;
    private Component[] _components;
    List<State> history = new List<State>();
    State lastState;

    struct State
    {
        public Vector2 position, scale;
        public Quaternion rotation;
        public Vector2 vel;
        public float angVel;
        public object[] values;
    }
    
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        var allScripts = GetComponents<MonoBehaviour>();
        var fieldList = new List<FieldInfo>();
        var compList = new List<Component>();

        foreach (var scr in allScripts)
        {
            foreach (var field in scr.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (field.IsDefined(typeof(TimeTrackedAtribute), false))
                {
                    fieldList.Add(field);
                    compList.Add(scr);
                }
            }
        }

        _fieldInfo = fieldList.ToArray();
        _components = compList.ToArray();
        
        TimeManager.updateRecEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        if (TimeManager.Instance.isRewinding)
            return;
        Record();
    }

    public void Record()
    {
        var state = new State
        {
            position = transform.position,
            rotation = transform.rotation,
            scale = transform.localScale,

            vel = _rigidbody2D ? _rigidbody2D.linearVelocity : Vector2.zero,
            angVel = _rigidbody2D ? _rigidbody2D.angularVelocity : 0f,
            values = new object[_fieldInfo.Length]
        };

        for (int i = 0; i < _fieldInfo.Length; i++)
        {
            state.values[i] = _fieldInfo[i].GetValue(_components[i]);
        }
        
        history.Add(state);
        
        if(history.Count > maxStates)
            history.RemoveAt(0);
            
    }

    public bool RewindStep()
    {
        if (history.Count == 0)
            return false;
        
        var state = history[^1];
        lastState = state;
        if (_rigidbody2D) _rigidbody2D.isKinematic = true;

        transform.position = state.position;
        transform.rotation = state.rotation;
        transform.localScale = state.scale;

        for (int i = 0; i < _fieldInfo.Length; i++)
        {
            _fieldInfo[i].SetValue(_components[i], state.values[i]);
        }
        
        history.RemoveAt(history.Count - 1);
        return true;
    }

    public void StopRewind()
    {
        if (!_rigidbody2D)
            return;
            
        _rigidbody2D.isKinematic = false;
        _rigidbody2D.linearVelocity = lastState.vel;
        _rigidbody2D.angularVelocity = lastState.angVel;
    }

    public void OnDestroy()
    {
        TimeManager.updateRecEvent?.Invoke();
    }
}
