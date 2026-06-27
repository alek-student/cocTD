using UnityEngine;

public class TowerScript : MonoBehaviour
{
    public float shootingCooldown = 1f;
    public float rangeRadius = 3f;
    public GameObject projectileInstance;
    TowerVisualsInterface visuals;

    private void Start()
    {
        visuals = GetComponent<TowerVisualsInterface>();
    }

    float remainingCooldown = 0f;
    void Update()
    {
        remainingCooldown -= Time.deltaTime;

        var hits = Physics2D.CircleCastAll(transform.position, rangeRadius, Vector2.zero, 0, (1 << 7));

        if (hits.Length == 0)
        {
            return;
        }

        float minDist = 99999999f;
        Vector2 targetPos = Vector2.zero;

        foreach (var hit in hits)
        {
            float dist = hit.transform.position.sqrMagnitude;
            if (dist < minDist)
            {
                targetPos = hit.transform.position;
                minDist = dist;
            }
        }

        Vector2 facingVector = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y);

        var rotation = Quaternion.Euler(0f, 0f, (Mathf.Atan2(facingVector.y, facingVector.x) * Mathf.Rad2Deg) - 90);

        visuals.Rotate(rotation);

        if (remainingCooldown <= 0)
        {
            Instantiate(projectileInstance, transform.position, rotation);
            visuals.Shoot(shootingCooldown);
            remainingCooldown = shootingCooldown;
        }
    }
}
