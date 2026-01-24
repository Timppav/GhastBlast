using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    EntityHealth _playerHealth;
    PlayerController _playerController;
    PlayerStaffController _playerStaffController;

    private void OnEnable()
    {
        _playerHealth = GetComponent<EntityHealth>();
        _playerController = GetComponent<PlayerController>();
        _playerStaffController = GetComponentInChildren<PlayerStaffController>();
        _playerHealth.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _playerHealth.OnDeath -= HandleDeath;
    }
    
    public void HandleDeath()
    {
        _playerController._animator.SetBool("isDead", true);
        _playerController.enabled = false;
        if (_playerStaffController != null)
            _playerStaffController.enabled = false;

        StartCoroutine(GameOverAfterDelay(2f));
    }

    IEnumerator GameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.GameOver();
    }
}
