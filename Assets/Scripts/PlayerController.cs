using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] SpriteRenderer _characterBody;
    [SerializeField] Animator _animator;
    Rigidbody2D _rb;


    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandlePlayerMovement();
    }

    private void HandlePlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        movement = Vector2.ClampMagnitude(movement, 1.0f);
        _rb.linearVelocity = movement * _movementSpeed;

        bool characterIsWalking = movement.magnitude > 0f;
        _animator.SetBool("isWalking", characterIsWalking);

        bool flipSprite = movement.x < 0f;
        _characterBody.flipX = flipSprite;
    }
}
