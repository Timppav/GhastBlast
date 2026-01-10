using UnityEngine;

public class PlayerStaffController : MonoBehaviour
{
    [SerializeField] float _fireRate;
    float _nextFireTime;

    void Update()
    {
        if (Time.timeScale == 0f) return; // Disable staff rotation & shooting if game is paused
        
        RotateStaff();

        // Shoots when fire button is pressed/held down
        if (Input.GetButton("Fire1") && Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + 1f / _fireRate; // Calculates the next time player can shoot again
            Shoot();
        }
    }

    void RotateStaff()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = (mousePosition - (Vector2)transform.position).normalized;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Shoot()
    {
        Debug.Log("SHOOT");
    }
}
