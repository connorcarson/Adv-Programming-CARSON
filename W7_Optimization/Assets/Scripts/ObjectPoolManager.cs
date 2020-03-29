using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject _objectContainer, _objectToPool,  _target;
    
    [SerializeField] 
    private int _maxObjects;

    [SerializeField] 
    private float _spawnSpeed, _movementSpeed;
    
    private readonly List<GameObject> _activeObjects = new List<GameObject>(); 
    private readonly List<GameObject> _inactiveObjects = new List<GameObject>();

    private ScreenBounds _screenBounds;
    
    private void Start()
    {
        _screenBounds = new ScreenBounds(Camera.main);
        
        MakeObjects(_objectToPool, _objectContainer.transform, _maxObjects, _inactiveObjects);
        
        InvokeRepeating(nameof(GetObject), 0, _spawnSpeed);
    }
    
    private void Update()
    {
        UpdateObjects(_activeObjects, _inactiveObjects, _target, _movementSpeed);
    }

    private static void UpdateObjects(List<GameObject> activeObjects, List<GameObject> inactiveObjects, GameObject target, float speed)
    {
        for (var i = 0; i < activeObjects.Count; i++)
        {
            activeObjects[i].transform.position = Vector3.MoveTowards(activeObjects[i].transform.position, target.transform.position, speed * Time.deltaTime);
            
            if (Mathf.Abs(activeObjects[i].transform.position.x - target.transform.position.x) < 0.01f &&
                Mathf.Abs(activeObjects[i].transform.position.y - target.transform.position.y) < 0.01f)
            {
                DeactivateObject(activeObjects[i], inactiveObjects, activeObjects);
            }
        }
    }
    
    private static void MakeObjects(GameObject toMake, Transform parent, int numberToMake, List<GameObject> inactive)
    {
        for (var i = 0; i < numberToMake; i++)
        {
            var newObject = Instantiate(toMake, parent);
            
            newObject.SetActive(false);
            
            inactive.Add(newObject);
        }
    }

    private void GetObject()
    {
        ActivateObject(_inactiveObjects, _activeObjects);
    }
    
    private void ActivateObject(List<GameObject> inactive, List<GameObject> active)
    {
        var toActivate = inactive[0];
        
        toActivate.transform.position = GetRandomPositionOffscreen(_screenBounds, 1);

        inactive.Remove(toActivate);
        active.Add(toActivate);
        
        toActivate.SetActive(true);
    }

    private static void DeactivateObject(GameObject toDeactivate, List<GameObject> inactive, List<GameObject> active)
    {
        toDeactivate.SetActive(false);
        
        active.Remove(toDeactivate);
        inactive.Add(toDeactivate);
    }

    private static Vector2 GetRandomPositionOffscreen(ScreenBounds screenBounds, float distanceFromBounds)
    {
        var toReturn = new Vector2();

        var randomNumber = Random.Range(0.0f, 1.0f);

        //approx. 25% of the time, spawn offscreen left
        if (randomNumber <= 0.25f) {
            toReturn.x = Random.Range(screenBounds.left - distanceFromBounds, screenBounds.left);
            toReturn.y = Random.Range(screenBounds.bottom - distanceFromBounds, screenBounds.top + distanceFromBounds);
        } 
        //approx. 25% of the time, spawn offscreen right
        else if (randomNumber > 0.25f && randomNumber <= 0.5f) {
            toReturn.x = Random.Range(screenBounds.right, screenBounds.right + distanceFromBounds);
            toReturn.y = Random.Range(screenBounds.bottom - distanceFromBounds, screenBounds.top + distanceFromBounds);
        } 
        //approx. 25% of the time, spawn offscreen bottom
        else if (randomNumber > 0.5f && randomNumber <= 0.75f) {
            toReturn.x = Random.Range(screenBounds.left - distanceFromBounds, screenBounds.right + distanceFromBounds);
            toReturn.y = Random.Range(screenBounds.bottom - distanceFromBounds, screenBounds.bottom);
        } 
        //approx. 25% of the time, spawn offscreen top
        else if (randomNumber > 0.75f && randomNumber <= 1.0f) {
            toReturn.x = Random.Range(screenBounds.left - distanceFromBounds, screenBounds.right + distanceFromBounds);
            toReturn.y = Random.Range(screenBounds.top, screenBounds.top + distanceFromBounds);
        }

        return toReturn;
    }
}


public class ScreenBounds
{
    private Camera camera;
    public readonly float left, right, top, bottom;

    public ScreenBounds(Camera camera)
    {
        this.camera = camera;
        var screenBounds = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));
        left = screenBounds.x;
        right = -screenBounds.x;
        bottom = screenBounds.y;
        top = -screenBounds.y;
    }
}
