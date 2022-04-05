using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class TankMotionScript : MonoBehaviour
{
    //[SerializeField] private GameObject tankModel = null;
    // [SerializeField] private PhotonView myPv;

    private void OnEnable()
    {
        this.gameObject.LeanMoveLocalY(0.25f, 2f).setEaseInOutCubic().setLoopPingPong();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.oxygenLevel += 100;
            GetComponent<PhotonView>().RPC("DestroyTank", RpcTarget.All);
        }
    }
    [PunRPC]
    void DestroyTank()
    {
        Debug.Log("destroy tank function called");
        Destroy(transform.parent.gameObject);

    }
}
