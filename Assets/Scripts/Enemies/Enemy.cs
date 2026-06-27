using UnityEngine;
using DG.Tweening;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] SpriteRenderer enemySprite;
    public int health = 3;
    public float speed = 0.5f;
    [HideInInspector] public Action<GameObject> deathCallback;
    [HideInInspector] public Action reachBaseCallback;

    public void TakeDamage(int damage)
    {
        health -= damage;

        enemySprite.DOKill();
        enemySprite.color = Color.red;

        if (health <= 0)
        {
            Die();
        }

        enemySprite.DOColor(Color.white, 0.1f).SetEase(Ease.Linear);
    }

    public void ReachBase()
    {
        reachBaseCallback();
        Die();
    }

    void Die()
    {
        deathCallback(gameObject);
        Destroy(gameObject);
    }
}
