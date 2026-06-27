using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker
{
    const int collisionLayer = 0;
    const int ignoreLayer = 6;

    public List<Collider2D> colliders = new();

    public bool CheckForCollisions()
    {
        foreach (var col in colliders)
        {
            if (Physics2D.OverlapCollider(col, new()) > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void SetCollidersToIgnoreLayer()
    {
        foreach (var col in colliders)
        {
            col.gameObject.layer = ignoreLayer;
        }
    }
    public void SetCollidersToActiveLayer()
    {
        foreach (var col in colliders)
        {
            col.gameObject.layer = collisionLayer;
        }
    }
}
