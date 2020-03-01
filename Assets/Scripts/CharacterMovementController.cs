using System.Collections;
using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    private bool _facingRight = true;
    private bool _isJumpTimeout;
    private bool _freezed;

    private LayerMask _groundLayerMask;

    private BoxCollider2D _collider;
    private GameObject _dustPuff;

    public float MaxMovementSpeed = 1f;
    public float JumpForce = 150f;
    public float JumpTimeoutSeconds = 0.5f;

    protected bool IsGrounded
    {
        get
        {
            Vector2 position = transform.position;
            Vector2 direction = Vector2.down;
            float distance = _collider.size.y + 0.01f;

            Debug.DrawRay(position, direction, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, _groundLayerMask);
            if (hit.collider != null)
            {
                return true;
            }

            return false;
        }
    }

    void Awake()
    {
        _groundLayerMask = 1 << LayerMask.NameToLayer("Ground");

        _collider = GetComponent<BoxCollider2D>();
        _dustPuff = (GameObject) Resources.Load("Prefabs/DustPuff");
    }

    void FixedUpdate()
    {
        if(_freezed)
        {
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            Flip(horizontalInput > 0);
        }

        var rigitbody = GetComponent<Rigidbody2D>();
        rigitbody.velocity = new Vector2(horizontalInput * MaxMovementSpeed, rigitbody.velocity.y);
        
        int verticalInput = (int) Input.GetAxisRaw("Vertical");
        if (!_isJumpTimeout && verticalInput == 1 && IsGrounded)
        {
            _isJumpTimeout = true;
            StartCoroutine(JumpTimeout());

            SpawnDust();
            MakeJump(rigitbody);
        }
    }

    public void SetFreezed(bool freezed)
    {
        _freezed = freezed;
    }

    private void MakeJump(Rigidbody2D rigitbody)
    {
        transform.parent = null;

        rigitbody.AddForce(new Vector2(0f, JumpForce));
    }

    private void SpawnDust()
    {
        GameObject dust = Instantiate(_dustPuff);
        dust.transform.position = new Vector3(transform.position.x, transform.position.y - _collider.size.y + 0.01f);

        var dustParticles = dust.GetComponent<ParticleSystem>();
        float totalDuration = dustParticles.main.duration + dustParticles.main.startLifetimeMultiplier;
        Destroy(dust, totalDuration);
    }

    private void Flip(bool isMovingRight)
    {
        if(isMovingRight == _facingRight)
        {
            return;
        }

        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private IEnumerator JumpTimeout()
    {
        yield return new WaitForSeconds(JumpTimeoutSeconds);

        _isJumpTimeout = false;
    }
}
