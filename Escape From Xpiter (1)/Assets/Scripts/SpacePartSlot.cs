using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpacePartSlot : MonoBehaviour, IDropHandler
{
    private float tweenTime = 0.5f;
    private Vector2 scaleVector = new Vector2(1.2f, 1.2f);
    private Vector2 normalVector = new Vector2(1f, 1f);
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {

            eventData.pointerDrag.GetComponent<Move>().droppedOnSlot = true;

            eventData.pointerDrag.LeanMove(gameObject.GetComponent<RectTransform>().position, 1.5f);
            //eventData.pointerDrag.transform.SetParent(innerScrollPanel);
            //  eventData.pointerDrag.GetComponent<RectTransform>().position = prevPos;
            //  prevPos = eventData.pointerDrag.GetComponent<Move>().prevPos;
            // eventData.pointerDrag.GetComponent<Move>().prevPos = eventData.pointerDrag.GetComponent<RectTransform>().position;

            LeanTween.scale(eventData.pointerDrag.gameObject, scaleVector, tweenTime - 0.4f).setEaseInQuad();
            LeanTween.scale(eventData.pointerDrag.gameObject, normalVector, tweenTime - 0.4f).setEaseOutQuad().delay = 0.1f;
            //OnPlace.Play();
            Debug.Log("Dropped on slot");
            Vector3 pos = gameObject.GetComponent<RectTransform>().position;
            eventData.pointerDrag.GetComponent<Move>().SetPosition(pos);

        }

    }
}
