using UnityEngine;
using Unity.Services.LevelPlay;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    [Header("LevelPlay")]
    private const string APP_KEY = "263801abd";
    private const string INTERSTITIAL_ID = "4q362gwk9ufbz6p0";
    private const string REWARDED_ID = "t7kzcmimzd5siwqy"; // ВСТАВИ СВОЙ

    private LevelPlayInterstitialAd interstitialAd;
    private LevelPlayRewardedAd rewardedAd;

    private int runCounter = 0;
    private const int SHOW_AD_EVERY = 3;

    private Action onRewardCallback;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LevelPlay.OnInitSuccess += OnInitSuccess;
        LevelPlay.OnInitFailed += OnInitFailed;

        Debug.Log("🚀 Init LevelPlay...");
        LevelPlay.Init(APP_KEY);
    }

    private void OnInitSuccess(LevelPlayConfiguration config)
    {
        Debug.Log("✅ LevelPlay Ready");

        CreateInterstitial();
        CreateRewarded();

        LoadInterstitial();
        LoadRewarded();
    }

    private void OnInitFailed(LevelPlayInitError error)
    {
        Debug.LogError($"❌ Init Error: {error}");
    }

    #region INTERSTITIAL

    private void CreateInterstitial()
    {
        interstitialAd = new LevelPlayInterstitialAd(INTERSTITIAL_ID);

        interstitialAd.OnAdLoaded += info => Debug.Log("Interstitial Loaded");
        interstitialAd.OnAdLoadFailed += error => Debug.LogError(error);
        interstitialAd.OnAdClosed += info =>
        {
            Debug.Log("Interstitial Closed");
            LoadInterstitial();
        };
    }

    private void LoadInterstitial()
    {
        interstitialAd?.LoadAd();
    }

    public void TryShowInterstitial()
    {
        runCounter++;

        if (runCounter >= SHOW_AD_EVERY)
        {
            ShowInterstitial();
            runCounter = 0;
        }
    }

    private void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
        }
        else
        {
            Debug.Log("Interstitial not ready");
        }
    }

    #endregion

    #region REWARDED

    private void CreateRewarded()
    {
        rewardedAd = new LevelPlayRewardedAd(REWARDED_ID);

        rewardedAd.OnAdLoaded += info => Debug.Log("Rewarded Loaded");
        rewardedAd.OnAdLoadFailed += error => Debug.LogError(error);

        rewardedAd.OnAdClosed += info =>
        {
            Debug.Log("Rewarded Closed");
            LoadRewarded();
        };

        rewardedAd.OnAdRewarded += (info, reward) =>
        {
            Debug.Log("🎁 Reward Given");
            onRewardCallback?.Invoke();
        };
    }

    private void LoadRewarded()
    {
        rewardedAd?.LoadAd();
    }

    public void ShowRewarded(Action onReward)
    {
        if (rewardedAd != null && rewardedAd.IsAdReady())
        {
            onRewardCallback = onReward;
            rewardedAd.ShowAd();
        }
        else
        {
            Debug.Log("Rewarded not ready");
        }
    }

    #endregion
}
