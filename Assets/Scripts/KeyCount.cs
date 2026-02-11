using UnityEngine;
using TMPro;

public class KeyCount : MonoBehaviour
{
    [Header("Key UI")]
    [SerializeField] TextMeshProUGUI _keyAmountText;

    Player _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        
        if (_player != null)
        {
            _player.OnKeysChanged += UpdateKeyUI;
            UpdateKeyUI(_player.GetKeyCount());
        }
    }

    void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnKeysChanged -= UpdateKeyUI;
        }
    }

    void UpdateKeyUI(int keyCount)
    {
        if (_keyAmountText != null)
        {
            _keyAmountText.text = string.Format("{0}/5", keyCount.ToString());
        }
    }
}
