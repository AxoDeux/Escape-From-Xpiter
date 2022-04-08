using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardCanvas : MonoBehaviour
{
    [SerializeField] private GameObject rewardIcon1 = null;
    [SerializeField] private GameObject rewardIcon2 = null;
    [SerializeField] private TextMeshProUGUI rewardText = null;
    [SerializeField] private GameObject[] rewardImages;

    //  [SerializeField] private string[] rewardStrings;
    [SerializeField] private TextMeshProUGUI spaceboxCount = null;

    private bool[] isRewarded;      //check whether the rewards are rewarded
    public static int count = 0;
    private void Awake()
    {
        count = 0;
        //Subscribe to spacebox events
        isRewarded = new bool[rewardImages.Length];
        for (int i = 0; i < rewardImages.Length; i++)         //setting all rewards bools to false
        {
            isRewarded[i] = false;
        }
        spaceboxCount.text = count.ToString();        
    }

    private void OnEnable()
    {
        //get a part and display whenever 
        for (int i = 0; i < rewardImages.Length; i++)
        {
            if (isRewarded[i]) { continue; }

            rewardImages[i].SetActive(true);
            rewardImages[i + 1].SetActive(true);

            rewardIcon1.GetComponent<Image>().sprite = rewardImages[i].GetComponent<Image>().sprite;
            rewardIcon2.GetComponent<Image>().sprite = rewardImages[i + 1].GetComponent<Image>().sprite;
            //  rewardText.text = rewardStrings[i];
            isRewarded[i] = true;
            isRewarded[i + 1] = true;

            count += 2;
            spaceboxCount.text = count.ToString();
            return;
        }

    }

}
