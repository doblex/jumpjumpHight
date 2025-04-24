using System;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] GameObject cannonBallPrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootDelay = 2f;
    float shootTimer = 0;

    private void Update()
    {
        shootTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            Vector3 dir = collision.gameObject.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            if (shootTimer <= 0)
            {
                shootTimer = shootDelay;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        Instantiate(cannonBallPrefab, shootPoint.position, shootPoint.rotation);
    }
}
