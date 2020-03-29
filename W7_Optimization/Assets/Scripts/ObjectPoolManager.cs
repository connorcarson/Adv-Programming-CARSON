using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{

    [SerializeField] 
    private GameObject _objectContainer, _objectToPool;

    [SerializeField] 
    private int maxObjects;
    
    private List<GameObject> _activeObjects = new List<GameObject>(); 
    private List<GameObject> _inactiveObjects = new List<GameObject>();

    private ScreenBounds _screenBounds;

    private void Start()
    {
        _screenBounds = new ScreenBounds(Camera.main);
        
        MakeObjects(_objectToPool, _objectContainer.transform, maxObjects, _inactiveObjects);
    }
    
    private void Update()
    {
        
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

    private GameObject GetObject(List<GameObject> inactive)
    {
        return inactive[0];
    }

    private static Vector2 GetRandomPositionOffscreen(ScreenBounds screenBounds, float distanceFromBounds)
    {
        Vector2 toReturn = new Vector2();

        float randomNumber = Random.Range(0, 1);
        
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
