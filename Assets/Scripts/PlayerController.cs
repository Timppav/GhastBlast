using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _movementSpeed;
    [SerializeField] SpriteRenderer _characterBody;
    [SerializeField] public Animator _animator;
    [SerializeField] AudioClip _footstep;
    Rigidbody2D _rb;

    float _nextFootstepAudio = 0f;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandlePlayerMovement();
        HandleSpriteFlip();
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

        if (characterIsWalking)
        {
            HandleWalkingSounds();
        }
    }

    void HandleWalkingSounds()
    {
        if (Time.time >= _nextFootstepAudio) // Check if it's time to play the next footstep sound
        {
            AudioManager.Instance.PlayAudio(_footstep, AudioManager.SoundType.SFX, 0.3f, false);

            // Calculate the next timestamp when a fooststep should be played if still walking
            // The audio frequency is calculated in a way that plays the audio twice during the walking animation loop
            float audioFrequency = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / 2f;
            _nextFootstepAudio = Time.time + audioFrequency;
        }
    }

    void HandleSpriteFlip()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool flipSprite = mousePosition.x < transform.position.x;
        _characterBody.flipX = flipSprite;
    }
}
