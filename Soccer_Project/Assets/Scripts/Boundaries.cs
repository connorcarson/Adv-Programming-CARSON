using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Boundaries
{
    private Vector3 _screenBounds;
    private float _verticalPadding = 0.65f;
    private float _horizontalPadding = 0.15f;
    public void Initialize()
    {
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Camera.main.transform.position.y, Screen.height));
    }
    
    public void Update()
    {
        foreach (var player in ServicesLocator.PlayerManager._players)
        {
            KeepInBounds(player.playerObject);
        }
        
        KeepInBounds(ServicesLocator.Ball.gameObject);
    }

    private void KeepInBounds(GameObject toCheck)
    {
        Vector3 inBounds = toCheck.transform.position;
        
        inBounds.x = Mathf.Clamp(inBounds.x, -1 * _screenBounds.x + _verticalPadding, _screenBounds.x - _verticalPadding);
        inBounds.z = Mathf.Clamp(inBounds.z, _screenBounds.z + _horizontalPadding, -1 * _screenBounds.z - _horizontalPadding);

        toCheck.transform.position = inBounds;
    }
}
