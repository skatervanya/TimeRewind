using System;
using UnityEngine;

public class PlayerRewind : MonoBehaviour
{
    [SerializeField] TimeManager _timeManager;
    [SerializeField] private Collider2D _portalCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            _timeManager.Rewind();
        }
    }
}
