using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float paddingX;
    [SerializeField] float paddingY;
    [SerializeField] float moveSpeed = 2f;

    [SerializeField] float moveRotationAngle = 25f;

    [SerializeField] float minAttackInterval;
    [SerializeField] float maxAttackInterval;

    [SerializeField] GameObject[] enemyBullets;
    [SerializeField] Transform enemyFirePoint;
    void OnEnable()
    {
        //StartCoroutine(RandomMoveCoroutine());
        //StartCoroutine(EnemyAttackCoroutine());
        StartCoroutine(nameof(RandomMoveCoroutine));
        StartCoroutine(nameof(EnemyAttackCoroutine));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator RandomMoveCoroutine()
    {
        transform.position = Viewport.Instance.EnemyRandomPosition(paddingX, paddingY);

        Vector3 targetPosition = Viewport.Instance.EnemyHalfRightMovePosition(paddingX, paddingY);

        while (gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position, targetPosition) > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                targetPosition = Viewport.Instance.EnemyHalfRightMovePosition(paddingX, paddingY);
            }
            yield return null;
        }
    }

    IEnumerator EnemyAttackCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minAttackInterval, maxAttackInterval));
            foreach (var enemyBullet in enemyBullets)
            {
                PoolManager.Release(enemyBullet, enemyFirePoint.position);
            }
        }
    }
}
