using UnityEngine;
using Photon.Pun;
using System.IO;

public class GameSetupController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Creating Player..");
        Vector3 playerRotation = new Vector3(0, 90, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.Euler(0,90,0));
    }
}
