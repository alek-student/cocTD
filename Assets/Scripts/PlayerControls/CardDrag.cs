using UnityEngine;
using DG.Tweening;

public class CardDrag : MonoBehaviour, DragInterface
{
    public EnemySpawner enemySpawner;
    public SpriteRenderer cardSprite;
    CollisionChecker collisionChecker = new();
    Transform hand;

    void Start()
    {
        hand = transform.parent;
        var colliders = transform.GetChild(0).GetComponentsInChildren<Collider2D>();
        collisionChecker.colliders.AddRange(colliders);
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

        cardSprite.color = spriteColour;
    }

    public void OnDragBegin()
    {
        transform.DOKill();
        transform.SetParent(null);
    }

    public void OnDragCancel()
    {
        transform.SetParent(hand);
        transform.DOLocalMove(new Vector2(transform.localPosition.x, 0.35f), 0.05f).SetEase(Ease.Linear);
        cardSprite.color = Color.white;
    }

    bool isActive = true;
    public void OnDragRelease()
    {
        if (collisionChecker.CheckForCollisions())
        {
            OnDragCancel();
            return;
        }

        enemySpawner.UpdateNavmesh();
        collisionChecker.SetCollidersToActiveLayer();

        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<SpriteRenderer>());
        transform.DOKill();
        transform.position = new Vector2(transform.position.x, transform.position.y);
        isActive = false;
        Destroy(this);
    }

    public void OnHoverBegin()
    {
        if (isActive)
        {
            transform.DOLocalMove(new Vector2(transform.localPosition.x, 1f), 0.1f).SetEase(Ease.Linear);
        }
        else
        {
            transform.DOKill();
        }
    }
    public void OnHoverEnd()
    {
        if (isActive)
        {
            transform.DOLocalMove(new Vector2(transform.localPosition.x, 0.35f), 0.1f).SetEase(Ease.Linear);
        }
        else
        {
            transform.DOKill();
        }
    }
}
