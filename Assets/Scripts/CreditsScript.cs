using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour {
    public Text text;
    public float textFadeInSpeed = 0.5f;
    public float textVisibleTime = 1.0f;
    public Animator cameraAnimator;
    public GameObject blockMousePanel;
    private Coroutine creditsCoroutine;
    void Start()
    {
        text.canvasRenderer.SetAlpha(0.0f);
        //StartCoroutine(CreditsRolling());
    }
    void Update()
    {

    }
    public void startCredits()
    {
        creditsCoroutine = StartCoroutine(CreditsRolling());
    }
    public void stopCredits()
    {
        StopCoroutine(creditsCoroutine);
        text.CrossFadeAlpha(0.0f, textFadeInSpeed, true);
        blockMousePanel.SetActive(false);
    }
    IEnumerator CreditsRolling()
    {
        text.text = "Thank you for playing\n\nAn original game by\nJonathan West";
        text.CrossFadeAlpha(1.0f, textFadeInSpeed, true);
        yield return new WaitForSeconds(textVisibleTime);
        text.CrossFadeAlpha(0.0f, textFadeInSpeed, true);
        yield return new WaitForSeconds(textFadeInSpeed);
        text.text = "Producer\nKen Miller\n\nArt Director\nKatrina Yi";
        text.CrossFadeAlpha(1.0f, textFadeInSpeed, true);
        yield return new WaitForSeconds(textVisibleTime);
        text.CrossFadeAlpha(0.0f, textFadeInSpeed, true);
        yield return new WaitForSeconds(textFadeInSpeed);
        text.text = "Programming\nKen Miller\nRuben Sanchez\nJuan Alvarez\nStanley Ung\nWolfgang Hellickson";
        text.CrossFadeAlpha(1.0f, textFadeInSpeed, true);
        yield return new WaitForSeconds(textVisibleTime);
        text.CrossFadeAlpha(0.0f, textFadeInSpeed, true);
        yield return new WaitForSeconds(textFadeInSpeed);
        text.text = "Art\nKatrina Yi\nNathan Xa\nSarah Cho\nChi Ngo\n\nSound Design and Composition\nDaniel Ramos\n\nGame Design\nJonathan West";
        text.CrossFadeAlpha(1.0f, textFadeInSpeed, true);
        yield return new WaitForSeconds(textVisibleTime);
        text.CrossFadeAlpha(0.0f, textFadeInSpeed, true);
        yield return new WaitForSeconds(textFadeInSpeed);
        cameraAnimator.SetBool("isOnCredits", false);
        blockMousePanel.SetActive(false);
    }
}
