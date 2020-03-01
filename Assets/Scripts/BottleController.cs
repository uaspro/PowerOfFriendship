using Photon.Pun;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    private const string PlayerTag = "Player";

    public float BottlePenetrationSpeed = 0.1f;
    public float MinColliderHeight = 0.05f;

    private CapsuleCollider2D _collider;
    private GameObject _bottleUser;

    void Awake()
    {
        _collider = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if(_bottleUser == null)
        {
            return;
        }

        if (_collider.size.y <= MinColliderHeight)
        {
            var characterController = _bottleUser.GetComponent<CharacterMovementController>();
            characterController.SetFreezed(false);

            Destroy(gameObject);

            return;
        }

        var deltaChage = new Vector2(0, BottlePenetrationSpeed * Time.deltaTime);
        _collider.size -= deltaChage;
        _collider.offset -= deltaChage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject;
        if (player.tag != PlayerTag)
        {
            return;
        }

        if(_bottleUser != null)
        {
            return;
        }

        if(player.transform.position.x >= transform.position.x - 0.03f && 
            player.transform.position.x <= transform.position.x + 0.03f)
        {
            _bottleUser = collision.gameObject;

            var characterController = _bottleUser.GetComponent<CharacterMovementController>();
            characterController.SetFreezed(true);
        }
    }
}
