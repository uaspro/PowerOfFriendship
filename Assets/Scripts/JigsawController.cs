using Photon.Pun;
using UnityEngine;

public class JigsawController : MonoBehaviour
{
    private Vector3 _startPosition;
    private bool _movingLeft = true;

    public float MovementSpeed = 3f;
    public float MaxDistance = 5f;

    void Awake()
    {
        _startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        var deltaChange = new Vector3(MovementSpeed * Time.deltaTime, 0);
        transform.position += _movingLeft ? -deltaChange : deltaChange;

        if (Mathf.Abs(transform.position.x - _startPosition.x) > MaxDistance)
        {
            _movingLeft = !_movingLeft;
        }
    }
}
