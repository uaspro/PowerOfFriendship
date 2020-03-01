using Photon.Pun;
using UnityEngine;

public class PlayerNetworking : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerCamera;

    [SerializeField]
    private MonoBehaviour[] _scriptsToIgnore;

    void Awake()
    {
        DisableNetworkSpecificFeatures();
    }

    private void DisableNetworkSpecificFeatures()
    {
        var photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            _playerCamera.SetActive(false);

            foreach(var script in _scriptsToIgnore)
            {
                script.enabled = false;
            }

            var rigitbody2d = GetComponent<Rigidbody2D>();
            rigitbody2d.gravityScale = 0;
        }
    }
}
