using UnityEngine;

public class Player : MonoBehaviour
{
    EntityHealth _playerHealth;

    private void OnEnable()
    {
        _playerHealth = GetComponent<EntityHealth>();
        _playerHealth.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _playerHealth.OnDeath -= HandleDeath;
    }
    
    public void HandleDeath()
    {
        GameManager.Instance.GameOver();
    }
}
