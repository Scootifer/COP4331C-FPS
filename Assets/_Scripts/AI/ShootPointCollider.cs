using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPointCollider : MonoBehaviour
{
    public List<GameObject> _collisionList { get; private set; }

    private void Start()
    {
        _collisionList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collisionList == null || other == null || other.gameObject == null) return;
        _collisionList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_collisionList == null || other == null || other.gameObject == null) return;
        _collisionList.Remove(other.gameObject);
    }
}
