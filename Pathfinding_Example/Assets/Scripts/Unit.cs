using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    
    [SerializeField]
    private float _speed = 5.0f;
    
    private Vector3[] _path;
    private int _targetIndex;

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool success)
    {
        if (success) {
            _path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = _path[0];

        while (true) {
            if (transform.position == currentWaypoint) {
                _targetIndex++;
                if (_targetIndex >= _path.Length) yield break;
                
                currentWaypoint = _path[_targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, _speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (GridManager.instance._gizmoSettings != GridManager.GizmoSettings.OnlyDisplaySimplifiedPathGizmos) return;

        if (_path == null) return;
        
        for (var i = _targetIndex; i < _path.Length; i++) {
            Gizmos.color = Color.black;
            
            Gizmos.DrawCube(_path[i], Vector3.one);

            if (i == _targetIndex) {
                Gizmos.DrawLine(transform.position, _path[i]);
            }
            else {
                Gizmos.DrawLine(_path[i - 1], _path[i]);
            }
        }
    }
}
