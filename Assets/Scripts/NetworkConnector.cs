using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkConnector : MonoBehaviourPunCallbacks
{
    private const string DefaultRoomName = "DefaultRoom";
    
    public GameObject StartSign;

    public string[] Characters;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom(DefaultRoomName, new RoomOptions { MaxPlayers = 3 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        int randomCharacterId = Random.Range(0, Characters.Length);
        PhotonNetwork.Instantiate($"Characters/{Characters[randomCharacterId]}", StartSign.transform.position, Quaternion.identity);
    }
}
