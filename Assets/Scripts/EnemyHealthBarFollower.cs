using UnityEngine;

public class HealthBarFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 1, 0);

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
