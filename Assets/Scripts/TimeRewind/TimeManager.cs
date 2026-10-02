using System;
using UnityEngine; 

public class TimeManager : MonoBehaviour
{
    public static  TimeManager Instance;
    public static Action updateRecEvent;

    public int rewindSpeed = 1;
    [SerializeField]public KeyCode toggleKey;
    
    [HideInInspector]public bool isRewinding;

    private TimeRecorder[] _recorders;

    private void Awake()
    {
        Instance = this;
        UpdateRecorderList();
        updateRecEvent = UpdateRecorderList;
    }

    public void Rewind()
    {
        isRewinding = !isRewinding; 
        if (!isRewinding) 
            StopRewindAll();
    }

    private void FixedUpdate()
    {
        if (!isRewinding)
            return;
        
        bool recorded = false;
        int steps = Mathf.Max(1, rewindSpeed);

        for (int i = 0; i < steps; i++)
        {
            foreach (var rec in _recorders)
            {
                if (rec.RewindStep())
                {
                    recorded = true;
                }
                
                if(!recorded) StopRewindAll();
            }
        }

    }

    public void StopRewindAll()
    {
        foreach (var rec in _recorders)
        {
            rec.StopRewind();
        }
        
        isRewinding = false;
    }
    
    private void UpdateRecorderList()
    {
        _recorders = FindObjectsOfType<TimeRecorder>();
    }
}
