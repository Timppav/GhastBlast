using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [SerializeField] AudioClip _musicClip;

    void Start()
    {
        if (_musicClip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySceneMusic(_musicClip);
        }
    }
}
