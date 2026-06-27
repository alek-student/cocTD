using UnityEngine;
using System.Collections.Generic;

public class CardHand : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public GameObject cardPrefab;
    public List<GameObject> obstablePrefabs;

    public float elementWidth = 2f;
    public float elementSpacing = 0.2f;

    void Start()
    {
        enemySpawner.UpdateNavmesh();
        DrawCard();
        DrawCard();
        DrawCard();
    }

    void Update()
    {
        float width = (transform.childCount * elementWidth) + ((transform.childCount - 1) * elementSpacing);

        float firstPosition = transform.position.x - (width / 2);

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.position = new Vector2(
                firstPosition + (i * (elementWidth + elementSpacing)) + (elementWidth / 2),
                child.position.y);
        }
    }

    public void DrawCard()
    {
        var cardObj = Instantiate(cardPrefab, new Vector2(0, -5), Quaternion.identity, transform);
        cardObj.transform.localPosition = new Vector2(0, 0.35f);

        var card = cardObj.GetComponent<CardDrag>();
        card.enemySpawner = enemySpawner;

        var obstacle = Instantiate(obstablePrefabs[Random.Range(0, obstablePrefabs.Count-1)], Vector2.zero, Quaternion.identity);
        obstacle.transform.SetParent(cardObj.transform);
        obstacle.transform.localPosition = Vector2.zero;

    }
}
