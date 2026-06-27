using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationRateDegrees = 15f;
    void Update()
    {
        transform.Rotate(new Vector3(0,0, Time.deltaTime * rotationRateDegrees));
    }
}
