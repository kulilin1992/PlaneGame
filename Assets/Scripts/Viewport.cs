using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    float minX;
    float maxX;
    float minY;
    float maxY;
    // Start is called before the first frame update
    void Start()
    {
        Camera mainCamera = Camera.main;
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1));

        minX = bottomLeft.x;
        maxX = topRight.x;
        minY = bottomLeft.y;
        maxY = topRight.y;
    }

    public Vector3 PlayerLimitPosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);
        return position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
