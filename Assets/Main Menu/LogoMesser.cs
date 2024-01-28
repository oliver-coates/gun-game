using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogoMesser : MonoBehaviour
{
    public Image logo;
    public RectTransform rct;

    public float alphaUpper;
    public float alphaLower;
    public float alphaTimeLower;
    public float alphaTimeUpper;

    private Vector2 startPos;
    public float driftMaxAmount;

    public void Awake()
    {
        startPos = rct.anchoredPosition;

        StartCoroutine(CRandomiseAlpha());
        StartCoroutine(CRandomisePosition());
    }

    private IEnumerator CRandomiseAlpha()
    {
        float startAlpha = logo.color.a;
        float targetAlpha = Random.Range(alphaLower, alphaUpper);
        float time = Random.Range(alphaTimeLower, alphaTimeUpper);

        float internalTimer = 0f;

        while (internalTimer < time)
        {
            internalTimer += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, internalTimer / alphaTimeLower);

            Color newColor = logo.color;
            newColor.a = alpha;

            logo.color = newColor;

            yield return new WaitForEndOfFrame();

        }

        StartCoroutine(CRandomiseAlpha());
    }

    private IEnumerator CRandomisePosition()
    {
        Vector2 startPosition = rct.anchoredPosition;
        
        Vector2 targetPosition = startPos;
        targetPosition += new Vector2(Random.Range(-driftMaxAmount, driftMaxAmount), Random.Range(-driftMaxAmount, driftMaxAmount));

        float internalTimer = 0f;
        float time = Random.Range(alphaTimeLower, alphaTimeUpper);

        while (internalTimer < time)
        {
            internalTimer += Time.deltaTime;

            float lerpAmount = internalTimer / time;
            Vector2 position = Vector2.Lerp(startPosition, targetPosition, lerpAmount);

            rct.anchoredPosition = position;

            yield return new WaitForEndOfFrame();

        }

        StartCoroutine(CRandomisePosition());
    }
}
