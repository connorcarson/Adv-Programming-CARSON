using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AspectRatioController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CanvasScaler _canvasScaler;
    [SerializeField] private Vector2 _aspectRatio;

    private void Awake()
    {
        _canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        AdjustToWindowSize(_aspectRatio.x, _aspectRatio.y, mainCamera, _canvasScaler);
    }
    
    private static void AdjustToWindowSize(float horizontalAspect, float verticalAspect, Camera toAdjust, CanvasScaler scaler)
    {
        float targetAspectRatio = horizontalAspect / verticalAspect;
        float windowAspectRatio = (float)Screen.width / (float)Screen.height;

        float scaleHeight = windowAspectRatio / targetAspectRatio;
        float scaleWidth = 1.0f / scaleHeight;

        Rect rect = toAdjust.rect;
        
        if (scaleHeight < 1.0f)
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }
        
        toAdjust.rect = rect;
        
        Vector2 newResolution = new Vector2(Screen.width * scaleWidth, Screen.height * scaleHeight);
        scaler.referenceResolution = newResolution;
    }
}