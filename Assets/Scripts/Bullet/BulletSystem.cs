using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSystem : MonoBehaviour
{

    [SerializeField] Bullet bullet;

    [SerializeField] float minBallisticAngle = -50f;
    [SerializeField] float maxBallisticAngle = 50f;

    float ballisticAngle;
    Vector3 targetDirection;
    public IEnumerator TrackCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);
                //向目标移动
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {   
                

                targetDirection = target.transform.position - transform.position;

                var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f, 0f, ballisticAngle);

                bullet.Move();
            }
            else
            {
                bullet.Move();
            }
            yield return null;
        }
    }
}
