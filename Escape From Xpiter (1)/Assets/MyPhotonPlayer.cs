using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class MyPhotonPlayer : MonoBehaviour
{
    PhotonView myPv;
    GameObject myPlayerAvatar;
    int myPlayerNumber = 0;
    Player[] allPlayers;
    // Start is called before the first frame update
    void Start()
    {
        myPv = GetComponent<PhotonView>();
        allPlayers = PhotonNetwork.PlayerList;
        foreach (Player player in allPlayers)
        {
            //Debug.Log(player.UserId);
            if (player == PhotonNetwork.LocalPlayer)
            {
                break;

            }
            myPlayerNumber++;
        }

        if (myPv.IsMine)
        {
            myPlayerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GamePlayer"),
            GameController.instance.spawnPoints[myPlayerNumber].position, Quaternion.identity);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Cameras"), Vector3.zero, Quaternion.identity);
            //  Debug.Log(PhotonNetwork.LocalPlayer.UserId);
        }
        //Debug.Log(allPlayers[myPlayerNumber].UserId);

        //Debug.Log(myPlayerNumber);
    }

}
