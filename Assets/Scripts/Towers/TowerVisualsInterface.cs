using UnityEngine;

public interface TowerVisualsInterface
{
    abstract void Rotate(Quaternion rotation);
    abstract void Shoot(float cooldown);
}
