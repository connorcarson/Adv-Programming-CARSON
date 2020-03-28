using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioController : MonoBehaviour
{
    [SerializeField] private float _horizontalAspect, _verticalAspect;

    private void Start()
    {
        Camera cam = GetComponent<Camera>();
        AdjustToWindowSize(_horizontalAspect, _verticalAspect, cam);
    }

    private static void AdjustToWindowSize(float horizontalAspect, float verticalAspect, Camera toAdjust)
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
    }
}
