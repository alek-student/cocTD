using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 2;
    public float speed = 5f;
    public float timeout = 10f;

    Collider2D collider;
    float timeLeft;
    private void Start()
    {
        timeLeft = timeout;
        collider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        List<Collider2D> results = new();
        if (Physics2D.OverlapCollider(collider, results) > 0)
        {
            Debug.Log("Hit");
            var enemy = results[0].gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            OnTimeout();
        }

    }

    protected virtual void OnTimeout(){}
}
