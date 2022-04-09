using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class TankMotionScript : MonoBehaviour
{
    //[SerializeField] private GameObject tankModel = null;
    // [SerializeField] private PhotonView myPv;

    //[SerializeField] private AudioSource oxygenAudio = null;

    private void OnEnable()
    {
        this.gameObject.LeanMoveLocalY(0.25f, 2f).setEaseInOutCubic().setLoopPingPong();
    }
    private void OnTriggerEnter(Collider other)
    {
        //oxygenAudio.Play();
        if (other.tag == "Player")
        {
            PlayerController.instance.oxygenLevel += 100;
            PlayerController.instance.PlayOxySound();
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
