using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        List<Collider2D> results = new();
        if (Physics2D.OverlapCollider(collider, results) > 0)
        {
            Debug.Log("Hit");
            foreach (var col in results)
            {
                var enemy = col.gameObject.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.ReachBase();
                }
            }
        }
    }
}
