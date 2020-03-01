using Photon.Pun;
using UnityEngine;

public class FallingPlatformController : MonoBehaviourPunCallbacks
{
    private Vector3 _startPosition;
    private bool _movingDownwards;

    public bool StartMovingDownwards = true;

    public float MovementSpeed = 1f;
    public float MaxDistance = 2f;

    void Awake()
    {
        _startPosition = transform.position;
        _movingDownwards = StartMovingDownwards;
    }

    void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        var deltaChange = new Vector3(0, MovementSpeed * Time.deltaTime);
        transform.position += _movingDownwards ? -deltaChange : deltaChange;

        if(Mathf.Abs(transform.position.y - _startPosition.y) > MaxDistance)
        {
            _movingDownwards = !_movingDownwards;
        }
    }
}
