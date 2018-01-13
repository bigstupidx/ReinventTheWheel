using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdvertisementsController : MonoBehaviour {
#if UNITY_ADS
    // Use this for initialization
    void Start () {
        Advertisement.Initialize("1668153");

    }

    public void ShowAdvertisement()
    {
        Advertisement.Show();
    }
#endif
}