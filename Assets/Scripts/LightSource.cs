using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSource : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] float _interactionDistance = 2f;
    [SerializeField] Animator _animator;

    [Header("Visual Feedback")]
    [SerializeField] GameObject _interactionPrompt;

    [SerializeField] Light2D _light;
    [SerializeField] GameObject _expOrbPrefab;
    [SerializeField] bool _isLit = false;

    Transform _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (_isLit)
        {
            LightUp();
        }
        
        if (_interactionPrompt != null)
        {
            _interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (_isLit) return;

        bool isInRange = IsPlayerInRange();

        if (_interactionPrompt != null)
        {
            _interactionPrompt.SetActive(isInRange);
        }

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            LightUp();
            DropExpOrb();

        }
    }

    bool IsPlayerInRange()
    {
        if (_player == null) return false;

        float distance = Vector3.Distance(transform.position, _player.position);
        return distance <= _interactionDistance;
    }

    void LightUp()
    {
        _isLit = true;

        if (_light != null)
        {
            _light.intensity = 0.8f;
        }

        if (_animator != null)
        {
            _animator.SetBool("isLit", true);
        }

        if (_interactionPrompt != null)
        {
            _interactionPrompt.SetActive(false);
        }
    }

    void DropExpOrb()
    {
        if (_expOrbPrefab != null)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector3 spawnPos = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            
            Instantiate(_expOrbPrefab, spawnPos, Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactionDistance);
    }
}
