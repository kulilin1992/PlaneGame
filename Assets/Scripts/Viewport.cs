using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewport : Singleton<Viewport>
{
    float minX;
    float maxX;
    float minY;
    float maxY;

    float middleX;

    public float MaxX => maxX;
    // Start is called before the first frame update
    void Start()
    {
        Camera mainCamera = Camera.main;
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1));

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;

        minX = bottomLeft.x;
        maxX = topRight.x;
        minY = bottomLeft.y;
        maxY = topRight.y;

        //middleX = (minX + maxX) / 2;
    }

    public Vector3 PlayerLimitPosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);
        return position;
    }

    public Vector3 EnemyRandomPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }

    public Vector3 EnemyHalfRightMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(middleX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;

    }
}
