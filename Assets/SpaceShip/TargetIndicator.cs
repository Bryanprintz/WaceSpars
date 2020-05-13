using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Transform target;
    
    [SerializeField] private float indicatorOffset;

    private Camera cam;
    
    
    private void Start()
    {
        cam = Camera.main;
        transform.SetParent(null);
    }

    private void Update()
    {
        if (target == null) return;

        var posOnScreen = cam.WorldToScreenPoint(target.position);

        if (isOnScreen(posOnScreen))
        {
            renderer.enabled = false;
        }
        
        else
        {
            RotateTowards(target.position);
            renderer.enabled = true;

            var screenPos = ClampPosition(posOnScreen);
            var worldPos = cam.ScreenToWorldPoint(screenPos);

            gameObject.transform.position = worldPos;
        }
        
    }
    
    private void RotateTowards(Vector2 target)
    {
        var offset = -90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }


    private bool isOnScreen(Vector2 v2)
    {
        var width = Screen.width;
        var height = Screen.height;

        if (v2.x > width || v2.x < 0) return false;
        if (v2.y > height || v2.y < 0) return false;
        
        return true;
    }
    
    private Vector3 ClampPosition(Vector3 target)
    {
        var IconSize = renderer.size;
        
        var maxX = Screen.width - IconSize.x;
        var minX = IconSize.x;

        var maxY = Screen.height - IconSize.y;
        var minY = IconSize.y;

        target.x = Mathf.Clamp(target.x, minX, maxX);
        target.y = Mathf.Clamp(target.y, minY, maxY);
        return target;
    }

}
