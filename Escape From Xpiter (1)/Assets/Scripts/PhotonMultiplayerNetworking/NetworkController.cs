using Photon.Pun;
using UnityEngine;

public class NetworkController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update

    void Start()
    {
        if (PhotonNetwork.IsConnected) { return; }
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {

        Debug.Log("Connected to " + PhotonNetwork.CloudRegion + "server !");

    }


}
