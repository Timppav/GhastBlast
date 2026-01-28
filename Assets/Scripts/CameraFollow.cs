using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] float _smoothSpeed = 0.125f;
    [SerializeField] Vector3 _offset = new Vector3(0, 0, -10);

    [Header("Cursor Offset")]
    [SerializeField] bool _useCursorOffset = true;
    [SerializeField] float _cursorInfluence = 0.2f;
    [SerializeField] float _maxCursorOffset = 5f;

    Camera _cam;
    Transform _player;

    void Start()
    {
        _cam = GetComponent<Camera>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) _player = playerObj.transform;
    }

    void LateUpdate()
    {
        if (_player == null) return;

        // Calculate base desired position
        Vector3 desiredPos = _player.position + _offset;

        // Add cursor _offset
        if (_useCursorOffset )
        {
            Vector3 cursorOffset = GetCursorOffset();
            desiredPos += cursorOffset;
        }

        // Interpolate to desired position smoothly
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, _smoothSpeed);
        transform.position = smoothedPos;
    }

    Vector3 GetCursorOffset()
    {
        // Get mouse position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(transform.position.z - _player.position.z);
        Vector3 worldMousePos = _cam.ScreenToWorldPoint(mousePos);

        // Calculate direction from player to mouse
        Vector3 direction = worldMousePos - _player.position;
        direction.z = 0;

        // Clamp the _offset magnitude
        float distance = Mathf.Min(direction.magnitude, _maxCursorOffset);
        Vector3 cursorOffset = direction.normalized * distance * _cursorInfluence;

        return cursorOffset;
    }
}
