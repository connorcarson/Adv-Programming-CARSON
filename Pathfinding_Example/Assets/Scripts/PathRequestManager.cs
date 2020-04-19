using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    private Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
    private PathRequest _currentPathRequest;

    private PathFinding _pathFinding;
    private static PathRequestManager _instance;
    
    private bool _isProcessingPath;

    private void Awake() {
        _instance = this;
        _pathFinding = GetComponent<PathFinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        _instance._pathRequestQueue.Enqueue(newRequest);
        _instance.TryProcessNext();
    }

    private void TryProcessNext() {
        if (!_isProcessingPath && _pathRequestQueue.Count > 0) {
            _currentPathRequest = _pathRequestQueue.Dequeue();
            _isProcessingPath = true;
            _pathFinding.StartFindPath(_currentPathRequest.pathStart, _currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success) {
        _currentPathRequest.callback(path, success);
        _isProcessingPath = false;
        TryProcessNext();
    }
    
    struct PathRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
            this.pathStart = pathStart;
            this.pathEnd = pathEnd;
            this.callback = callback;
        }
    }
}
