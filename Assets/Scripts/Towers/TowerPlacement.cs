using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class TowerPlacement : MonoBehaviour, DragInterface
{
    public GameObject towerPrefab;
    public GameObject previewObject;
    public Transform previewCircle;
    public SpriteRenderer iconSprite;
    public EnemySpawner enemySpawner;
    public List<SpriteRenderer> previewSprites;
    Vector2 initialScale;
    CollisionChecker collisionChecker = new();

    void Start()
    {
        initialScale = transform.localScale;
        var radius = towerPrefab.GetComponent<TowerScript>().rangeRadius;
        previewCircle.localScale *= radius;
        collisionChecker.colliders.Add(previewObject.GetComponent<Collider2D>());
        Debug.Log(collisionChecker.colliders.Count);
        collisionChecker.SetCollidersToIgnoreLayer();
    }

    public void OnDrag()
    {
        Vector2 mouseWorldPosition = InteractionHelpers.GetMousePosition();
        transform.position = new Vector3(
            Mathf.Lerp(mouseWorldPosition.x, transform.position.x, 0.3f),
            Mathf.Lerp(mouseWorldPosition.y, transform.position.y, 0.3f),
            -5f);

        Color spriteColour = collisionChecker.CheckForCollisions() ? Color.red : Color.white;

        foreach (var sprite in previewSprites)
        {
            sprite.color = spriteColour;
        }
    }

    public void OnDragBegin()
    {
        transform.DOKill();
        previewObject.SetActive(true);
        iconSprite.DOKill();
        iconSprite.DOFade(0, 0.1f);
    }

    public void OnDragCancel()
    {
        previewObject.SetActive(false);
        transform.DOLocalMove(new Vector3(0, 0, -0.5f), 0.05f).SetEase(Ease.Linear);
        iconSprite.DOKill();
        iconSprite.DOFade(1, 0.5f);
    }

    public void OnDragRelease()
    {
        if (!collisionChecker.CheckForCollisions())
        {
            Instantiate(towerPrefab, new Vector2(transform.position.x, transform.position.y), quaternion.identity);
            enemySpawner.UpdateNavmesh();
        }
        previewObject.SetActive(false);
        OnDragCancel();
    }

    public void OnHoverBegin()
    {
        transform.DOScale(initialScale * 1.2f, 0.1f).SetEase(Ease.Linear);
    }

    public void OnHoverEnd()
    {
        transform.DOScale(initialScale, 0.1f).SetEase(Ease.Linear);
    }
}
