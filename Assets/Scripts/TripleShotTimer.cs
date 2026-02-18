using UnityEngine;
using TMPro;

public class TripleShotTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText;

    float _remainingTime;
    bool _isActive = false;

    void Update()
    {
        if (!_isActive) return;

        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0)
        {
            _remainingTime = 0;
            _isActive = false;
            OnTimerExpired?.Invoke();
        }

        _timerText.text = Mathf.CeilToInt(_remainingTime).ToString();
    }

    public System.Action OnTimerExpired;

    public void StartTimer(float duration)
    {
        _remainingTime = duration;
        _isActive = true;
    }

    public void AddTime(float duration)
    {
        _remainingTime += duration;
        _isActive = true;
    }

    public bool IsActive()
    {
        return _isActive;
    }
}
