using Photon.Pun;
using UnityEngine;

public class CharacterPositioningController : MonoBehaviourPunCallbacks, IPunObservable
{
    private const string MovingPlatformTag = "MovingPlatform";

    private LayerMask _groundLayerMask;

    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody2d;

    private bool _firstTake = true;

    private float _distance;

    private Vector3 _direction;

    private Vector3 _storedPosition;
    private Vector3 _networkPosition;

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
        _groundLayerMask = _groundLayerMask = 1 << LayerMask.NameToLayer("Ground");

        _collider = GetComponent<BoxCollider2D>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            var targetPosition = _networkPosition;
            if (transform.parent != null)
            {
                targetPosition.y = transform.position.y;
                transform.position = Vector3.Lerp(transform.position, targetPosition, 10);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _distance * (1.0f / PhotonNetwork.SerializationRate));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != MovingPlatformTag || !IsGrounded)
        {
            return;
        }

        transform.parent = collision.gameObject.transform;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform != transform.parent)
        {
            return;
        }

        transform.parent = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            _direction = transform.position - _storedPosition;
            _storedPosition = transform.position;

            stream.SendNext(transform.position);
            stream.SendNext(_direction);

            stream.SendNext(transform.localScale);
        }
        else
        {
            _networkPosition = (Vector3) stream.ReceiveNext();
            _direction = (Vector3) stream.ReceiveNext();

            if (_firstTake)
            {
                transform.position = _networkPosition;
                _distance = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                _networkPosition += _direction * lag;

                _distance = Vector3.Distance(transform.position, _networkPosition);
            }

            transform.localScale = (Vector3)stream.ReceiveNext();

            if (_firstTake)
            {
                _firstTake = false;
            }
        }
    }
}
