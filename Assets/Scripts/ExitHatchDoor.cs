using UnityEngine;

public class ExitHatchDoor : MonoBehaviour
{
    [SerializeField] int _requiredKeys = 5;

    [Header("Audio")]
    [SerializeField] AudioClip _doorLockedSound;

    bool _isUnlocked = false;
    Player _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        
        if (_player != null)
        {
            _player.OnKeysChanged += CheckDoorStatus;
            CheckDoorStatus(_player.GetKeyCount());
        }
    }

    void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnKeysChanged -= CheckDoorStatus;
        }
    }

    void CheckDoorStatus(int keyCount)
    {
        if (keyCount >= _requiredKeys && !_isUnlocked)
        {
            UnlockDoor();
        }
    }

    void UnlockDoor()
    {
        _isUnlocked = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (_isUnlocked)
        {
            GameManager.Instance.Victory();
        }

        if (_doorLockedSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAudio(_doorLockedSound, AudioManager.SoundType.SFX, 1.0f, false);
        }

        Debug.Log($"Door locked! Need {_requiredKeys - _player.GetKeyCount()} more keys.");
    }
}
