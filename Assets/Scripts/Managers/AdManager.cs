using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class AdManager : MonoBehaviour
{
    int count;
    float timeAtLastAd;

    public int levelsPerAd = 5;
    public int secPerAd = 60;

#if UNITY_IOS
    private string gameID = "3907258";
#elif UNITY_ANDROID
    private string gameID = "3907259";
#endif

    public bool testMode = true;

    public static AdManager instance;

    public void Awake()
    {


        if (instance == null)
        {
            instance = this;
        }else if(instance != this)
        {
            Debug.Log("Found another AdManager, deleting this");
            Destroy(gameObject);
        }

        if (Advertisement.isSupported == false)
        {
            //Ads are not supported, So no need to initilize ads
            return;
        }

        //We need to initialize ads if we can run them
        Advertisement.Initialize(gameID, testMode);

        ClearAdTimer();
    }

    /// <summary>
    /// Run an ad if we have either played more than x levels or if we have been playing longer than y time (in seconds)
    /// </summary>
    public void RunAd()
    {
        count += 1;

        Debug.Log("Test");
        if (Advertisement.isInitialized == false)
        {
            Debug.LogWarning("Ads are not initialized");
            return;
        }

        if (CheckIfCanRunAd())
        {
            Debug.LogWarning("Ad");
            Analytics.CustomEvent("Ad Run");
            Advertisement.Show();

            ClearAdTimer();
        }
    }

    public void ForceAd()
    {
        if (Advertisement.isInitialized == false)
        {
            return;
        }
        Advertisement.Show();
        Analytics.CustomEvent("Ad Run");
        ClearAdTimer();
    }

    public void ClearAdTimer()
    {
        count = 0;
        timeAtLastAd = Time.time;
    }

    public bool CheckIfCanRunAd()
    {
        //return true;
        if (count >= levelsPerAd)
        {
            return true;
        }

        if (Time.time - timeAtLastAd > secPerAd)
        {
            return true;
        }

        return false;
    }

    public bool IsAdRunning()
    {
        return Advertisement.isShowing;
    }

}
