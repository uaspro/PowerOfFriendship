using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        var rigitbody = GetComponent<Rigidbody2D>();
        _animator.SetFloat("Speed", Mathf.Abs(rigitbody.velocity.x));
    }
}
