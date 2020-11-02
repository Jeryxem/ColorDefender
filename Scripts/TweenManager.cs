using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TweenManager : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] float xPos;
    [SerializeField] float yPos;

    public void MoveInFromLeft(GameObject selectedObject)
    {
        LeanTween.moveLocalX(selectedObject, 0f, duration).setOnComplete(GameManager.instance.PauseGame);
    }

    public void MoveOutToRight(GameObject selectedObject)
    {
        LeanTween.moveLocalX(selectedObject, 1920f, duration).setOnComplete(GameManager.instance.LoadLoadingScene);
    }

    public void MoveOutToTop(GameObject selectedObject)
    {
        LeanTween.moveLocalY(selectedObject, 1080f, duration).setOnComplete(GameManager.instance.MoveBackground);
    }

    public void MoveOutToBottom(GameObject selectedObject)
    {
        LeanTween.moveLocalY(selectedObject, -1080f, duration).setOnComplete(GameManager.instance.StartGame);
    }

    public void BackgroundMoveRight()
    {
        LeanTween.moveX(gameObject, 30f, 10f).setOnComplete(BackgroundMoveLeft);
    }

    public void BackgroundMoveLeft()
    {
        LeanTween.moveX(gameObject, -80f, 10f).setOnComplete(BackgroundMoveRight);
    }

    public void ScaleUp(GameObject selectedObject)
    {
        LeanTween.scale(selectedObject, new Vector3(1.2f, 1.2f, 1.2f), duration).setOnComplete(ScaleDown);
    }

    public void ScaleDown()
    {
        GameObject costText = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        LeanTween.scale(costText, new Vector3(1f, 1f, 1f), 0.5f);
    }
}
