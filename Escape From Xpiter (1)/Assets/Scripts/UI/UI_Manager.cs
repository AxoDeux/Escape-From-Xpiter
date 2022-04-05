using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private Slider oxygenLevel;
    [SerializeField] private Image OxygenBar;
    [SerializeField] private TextMeshProUGUI energyCounttext;
    [SerializeField] private TextMeshProUGUI timeText = null;
    [SerializeField] private float timeLimit;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private float maxOxygenLevel;

    [SerializeField] private GameObject optionsCanvas = null;
    [SerializeField] private TMP_Text energyInput = null;
    [SerializeField] private TMP_Text timeInput = null;

    [SerializeField] private AudioSource buttonClick = null;
    [SerializeField] private AudioSource ambienceSound = null;

    private bool soundOn = true;

    public static event Action<int> OptionsUpdated;
    public static event Action MapViewCalled;

    private GameObject playerCanvas = null;
    private PhotonView photonView;
    private bool isMapUsed = false;

    public static float _time { get; private set; }
    public static bool isTimeUp { get; private set; }
    private float freezeTime;
    private bool isChecking = false;
    [SerializeField] private GameObject[] jigsawParts;

    void Start()
    {
        // maxOxygenLevel = PlayerController.instance.oxygenLevel;
        isTimeUp = false;
        energyCounttext.text = PlayerController.totalMoves.ToString();
        oxygenLevel.maxValue = maxOxygenLevel;
        _time = timeLimit;

        playerCanvas = this.gameObject;
        photonView = GetComponent<PhotonView>();
        freezeTime = 20f;
    }

    void Update()
    {

        if (RewardCanvas.count == 12)
        {
            if (Move.timeToRepair <= 0f)
            {
                if (!isChecking)
                {
                    isChecking = true;
                    if (isGameCompleted())
                    {
                        photonView.RPC(nameof(RPC_GameOver), RpcTarget.All);
                    }
                }
            }
        }
        if (Move.timeToRepair > 0f)
        {
            Move.timeToRepair -= Time.deltaTime;

            //  Debug.Log("Time to repair text updated!");
        }

        energyCounttext.text = PlayerController.totalMoves.ToString();

        Timer();
        UpdateOxygenLevel();
    }

    public void OnClickOptions()
    {
        buttonClick.Play();
        optionsCanvas.SetActive(true);
    }

    private void Timer()
    {
        _time -= Time.deltaTime;

        int seconds = (int)_time % 60;
        int minutes = (int)_time / 60;

        if (PhotonNetwork.IsMasterClient)
        {
            if (_time <= 0)
            {
                _time = timeLimit;
                //Debug.Log("Time is negative");
                photonView.RPC(nameof(RPC_GameOver), RpcTarget.All);
            }
        }

        if (seconds < 10)
        {
            timeText.text = $"0{minutes} : 0{seconds}";
        }
        else
        {
            timeText.text = $"0{minutes} : {seconds}";
        }
    }

    void UpdateOxygenLevel()
    {


        if (oxygenLevel.value <= 0f)
        {
            oxygenLevel.value = 250f;
            // gameOverCanvas.SetActive(true);
            if (PlayerController.instance.playerInput.currentActionMap.enabled)
            {
                PlayerController.instance.playerInput.currentActionMap.Disable();
                StartCoroutine(FreezePlayer(freezeTime));
            }

        }
        else if (oxygenLevel.value <= 90f)
        {
            OxygenBar.color = Color.red;

        }
        else
        {
            OxygenBar.color = Color.green;

        }
        oxygenLevel.value = PlayerController.instance.oxygenLevel;


    }
    bool isGameCompleted()
    {
        int i = 0;
        for (i = 0; i < jigsawParts.Length; i++)
        {
            if (!jigsawParts[i].GetComponent<Move>().isPositionCorrect)
            {
                break;
            }
        }
        if (i == 12)
        {
            return true;
        }
        else
        {
            isChecking = false;
            return false;
        }
    }
    IEnumerator FreezePlayer(float freeze_time)
    {
        yield return new WaitForSeconds(freeze_time);
        PlayerController.instance.oxygenLevel = 100f;
        oxygenLevel.value = 100f;
        PlayerController.instance.playerInput.currentActionMap.Enable();
    }

    [PunRPC]
    private void RPC_GameOver()
    {
        if (RewardCanvas.count != 12)
        {
            isTimeUp = true;
        }
        gameOverCanvas.SetActive(true);
        this.enabled = false;
        //playerCanvas.SetActive(false);
    }


    public void OnEndTimeEdit(TMP_InputField value)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("changing time");
            float time = float.Parse(value.text);
            photonView.RPC(nameof(ChangeTime), RpcTarget.All, time);
        }
    }

    public void OnEndEnergyEdit(TMP_InputField value)
    {
        int energy = int.Parse(value.text);
        OptionsUpdated?.Invoke(energy);
    }

    public void OnCloseOptions()
    {
        optionsCanvas.SetActive(false);
    }

    [PunRPC]
    private void ChangeTime(float time)
    {
        _time = time;
    }
    public void LeaveRoom()
    {
        buttonClick.Play();
        StartCoroutine(LeaveRoom_and_Load());

    }
    private IEnumerator LeaveRoom_and_Load()
    {
        Debug.Log("func called");
        PhotonNetwork.LeaveRoom();
        Debug.Log("Leave room called");
        while (PhotonNetwork.InRoom)
            yield return null;
        this.gameObject.SetActive(false);
        Debug.Log("Game over canvas disabled");
        SceneManager.LoadScene(0);
    }

    public void MapViewClick(Button mapButton)
    {
        Debug.Log("Hello");
        MapViewCalled?.Invoke();
        mapButton.interactable = false;

        //photonView.RPC(nameof(ChangeView), RpcTarget.All);
    }

    /*[PunRPC]
    private void ChangeView()
    {
        MapViewCalled.Invoke();
        isMapUsed = true;
    }*/

    public void AudioToggle()
    {
        if (soundOn)
        {
            soundOn = false;
            ambienceSound.volume = 0;
        }
        else
        {
            soundOn = true;
            ambienceSound.volume = 0.3f;
        }
    }
}
