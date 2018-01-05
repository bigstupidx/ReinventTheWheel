using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Waits for the first tap/click of the round at which point it starts all countdowns/animations

public class RoundStartController : MonoBehaviour
{
    public GameObject tutorialContainer;
    public MammothController mammoth;
    public CameraController cameraCon;
    public HoleMaker hMaker;
    public GameObject sunTimer;
    public AudioSource countdownAudioSource;
    public AudioClip countdownClip1, countdownClip2, countdownClip3, chiselingClip;

    //used to check if the player has chiseled for the first time yet, if they have, then the sun timer animatio begins
    private bool _hasChiseled;
    private AudioSource _audio;
    // Use this for initialization
    void Start ()
    {
        _hasChiseled = false;
        _audio = GetComponent<AudioSource>();

        if(PlayerPrefs.GetInt("Tutorial", 0) != 0)
        {
            tutorialContainer.GetComponent<Animator>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetMouseButtonDown(0) && PlayerPrefs.GetInt("Tutorial", 0) != 0)
        {
            //begin sun timer animation on the first click/touch
            if (!_hasChiseled)
            {
                _hasChiseled = true;

                _audio.clip = chiselingClip;
                _audio.loop = false;
                _audio.Play();
                //begin countdowns and animations 
                Invoke("ReleaseTheMammoth", hMaker.timeToChisel);
                Invoke("StartCountDownAudio", hMaker.timeToChisel - 3);
                cameraCon.ChangeState();
                sunTimer.GetComponent<Animator>().SetTrigger("Start");
            }
        }

    }

    //After the timer, activate gravity on the boulder to start rolling
    public void ReleaseTheMammoth()
    {
        HoleMaker.activated = false;
        hMaker.chisel.SetActive(false);
        hMaker.enabled = false;
        mammoth.ReleaseTheMammoth();
    }

    private void StartCountDownAudio()
    {
        StartCoroutine(CountDownAudioPlay());
    }

    IEnumerator CountDownAudioPlay()
    {
        countdownAudioSource.clip = countdownClip1;
        countdownAudioSource.Play();
        yield return new WaitForSeconds(1);
        countdownAudioSource.Stop();
        countdownAudioSource.clip = countdownClip2;
        countdownAudioSource.Play();
        yield return new WaitForSeconds(1);
        countdownAudioSource.Stop();
        countdownAudioSource.clip = countdownClip3;
        countdownAudioSource.Play();
        yield return null;
    }
}
