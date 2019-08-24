using UnityEngine;
using System.Collections.Generic;

public class AverageScaleOutput : MonoBehaviour
{
    public Transform objectToApplyOutput;
    public Vector3 baseLocalScale = new Vector3(10, 10, 10);
    public float averageMultiplier = 1.0f;
    public float lerpSpeed = 1.2f;

    [Space(10)]
    public List<Transform> playerCharactersTransforms = new List<Transform>();

    public Transform moveThis;

    Vector3 sumOfAll = Vector3.zero;
    Vector3 averageResult = Vector3.zero;

    Vector3 posOfAll = Vector3.zero;
    Vector3 averagePosResult = Vector3.zero;

    public bool trackPosition;

    public void FixedUpdate()
    {
        if (GameManager.Instance.gameOver == false)
        {
            CalculateAndOutput();
        }
        if (trackPosition == true)
        {
            CalculatePosition();
        }

    }

    public void Update()
    {
        if (GameManager.Instance.gameOver == true)
        {
            returnToOriginalScale();
            // objectToApplyOutput.localScale = new Vector3(7.5f, 7.5f, 7.5f);
        }

    }

    public void CalculateAndOutput()
    {
        sumOfAll.x = sumOfAll.y = sumOfAll.z = 0;

        for (int i = 0; i < playerCharactersTransforms.Count; i++)
        {
            Transform t = playerCharactersTransforms[i];
            sumOfAll += t.localScale;
        }

        if (playerCharactersTransforms.Count > 0)
            averageResult = sumOfAll / playerCharactersTransforms.Count;

        objectToApplyOutput.localScale = Vector3.Lerp(objectToApplyOutput.localScale, baseLocalScale + (averageResult * averageMultiplier), Time.deltaTime * lerpSpeed);
    }

    public void AddPlayer(Transform t)
    {
        playerCharactersTransforms.Add(t);
        CalculateAndOutput();
    }

    public void RemovePlayer(Transform t)
    {
        playerCharactersTransforms.Remove(t);
        CalculateAndOutput();
    }

    public void CalculatePosition()
    {
        posOfAll.x = posOfAll.y = posOfAll.z = 0;

        for (int i = 0; i < playerCharactersTransforms.Count; i++)
        {
            Transform t = playerCharactersTransforms[i];
            posOfAll += t.localPosition;
        }
        if (playerCharactersTransforms.Count > 0)
        {
            averagePosResult = posOfAll / playerCharactersTransforms.Count;
            moveThis.position = averagePosResult;
        }
    }

    public void returnToOriginalScale()
    {
        sumOfAll.x = sumOfAll.y = sumOfAll.z = 0;

        for (int i = 0; i < playerCharactersTransforms.Count; i++)
        {
            Transform t = playerCharactersTransforms[i];
            sumOfAll += t.localScale;
        }

        if (playerCharactersTransforms.Count > 0)
            averageResult = sumOfAll / playerCharactersTransforms.Count;

        objectToApplyOutput.localScale = Vector3.Lerp(objectToApplyOutput.localScale, baseLocalScale * .5f, Time.deltaTime * lerpSpeed);
    }


}
