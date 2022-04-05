using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingCameraMovement : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitingInterval());
    }

    private IEnumerator WaitingInterval()
    {
        yield return new WaitForSeconds(10f);

        LeanTween.move(gameObject, new Vector3(41, 16, 31), 5f);

        yield return new WaitForSeconds(5f);

        LeanTween.move(gameObject, new Vector3(51, 21, 31), 5f).setEaseInCubic();
    }

}
