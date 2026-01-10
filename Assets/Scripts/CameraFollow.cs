using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform player;

    [Header("Follow Settings")]
    [SerializeField] float smoothSpeed = 0.125f;
    [SerializeField] Vector3 offset = new Vector3(0, 0, -10);

    [Header("Cursor Offset")]
    [SerializeField] bool useCursorOffset = true;
    [SerializeField] float cursorInfluence = 0.2f;
    [SerializeField] float maxCursorOffset = 5f;

    Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        // If there is no player assigned, attempt to find it
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Calculate base desired position
        Vector3 desiredPos = player.position + offset;

        // Add cursor offset
        if (useCursorOffset)
        {
            Vector3 cursorOffset = GetCursorOffset();
            desiredPos += cursorOffset;
        }

        // Interpolate to desired position smoothly
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothedPos;
    }

    Vector3 GetCursorOffset()
    {
        // Get mouse position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(transform.position.z - player.position.z);
        Vector3 worldMousePos = cam.ScreenToWorldPoint(mousePos);

        // Calculate direction from player to mouse
        Vector3 direction = worldMousePos - player.position;
        direction.z = 0;

        // Clamp the offset magnitude
        float distance = Mathf.Min(direction.magnitude, maxCursorOffset);
        Vector3 cursorOffset = direction.normalized * distance * cursorInfluence;

        return cursorOffset;
    }
}
