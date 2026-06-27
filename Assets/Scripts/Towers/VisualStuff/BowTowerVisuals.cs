using UnityEngine;

public class BowTowerVisuals : MonoBehaviour, TowerVisualsInterface
{
    public GameObject bowDrawn;
    public GameObject bowShot;
    public Transform rotationBase;
    public void Rotate(Quaternion rotation)
    {
        rotationBase.rotation = rotation;
    }

    public void Shoot(float cooldown)
    {
        bowDrawn.SetActive(false);
        bowShot.SetActive(true);
        Invoke("ArmBow", cooldown * 0.5f);
    }
    private void ArmBow()
    {
        bowShot.SetActive(false);
        bowDrawn.SetActive(true);
    }
}
