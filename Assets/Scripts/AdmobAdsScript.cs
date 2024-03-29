using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine.UI;

public class AdmobAdsScript : MonoBehaviour
{
    public TextMeshProUGUI totalCoins;
    BannerView bannerView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;
    NativeAd nativeAd;
    public Image img;

    public TextAsset jsonFile;
    [Space(20)]
    [Header("Ad IDs")]
    public string appId;
    public string bnr,itr,rew,nat;


    void GetJsonData()
    {
        JsonReader jsRead = JsonUtility.FromJson<JsonReader>(jsonFile.text);
        bnr = jsRead.Android.bannerId;
        itr = jsRead.Android.interId;
        rew = jsRead.Android.rewardedId;
        nat = jsRead.Android.nativeId;  

        JsonIndividualData readData = JsonUtility.FromJson<JsonIndividualData>(jsonFile.text);  

        appId = readData.appId;
        
    }
    void Start()
    {
        GetJsonData();

        AdsInitializeInStart();
    }

    public void AdsInitializeInStart()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {
            print("Ads Initialised !!");
        });
    }

#region Banner

    public void LoadBannerAd(){
        //create a banner
        CreateBannerView();

        //Listen to banner events
        ListenToBannerEvents();

        //load the banner
        if(bannerView == null)
            CreateBannerView();

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        print("Loading banner Ad !!");
        bannerView.LoadAd(adRequest); //Show the banner on the screen

    }
    void CreateBannerView()
    {
        if(bannerView!=null)
        {DestroyBannerAd();}
        bannerView = new BannerView(bnr,AdSize.Banner,AdPosition.Bottom);
    }
    void ListenToBannerEvents() 
    {
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}."+
                adValue.Value+
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
    public void DestroyBannerAd()
    {
        if(bannerView != null)
        {
            print("Destroying banner Ad");
            bannerView.Destroy();
            bannerView = null;
        }
    }

#endregion

#region Interstitial

    public void LoadInterstitialAd()
    {
        if(interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(itr,adRequest,(InterstitialAd ad,LoadAdError error) => 
        {
            if(error != null || ad == null)
            {
                print("Interstitial ad failed to load " + error);
                return;
            }
            print("Interstitial ad loaded !!" + ad.GetResponseInfo());

            interstitialAd = ad;
            InterstitialEvent(interstitialAd);
        });

    }
    public void ShowInterstitialAd()
    {
        if(interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            print("Interstitial is not Ready !!");
        }
    }
    public void InterstitialEvent(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) => 
        {
            Debug.Log("Interstitial ad paid {0} {1}."+
                adValue.Value+
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

#endregion

#region Rewarded
    public void LoadRewardedAd()
    {
        if(rewardedAd !=null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rew,adRequest,(RewardedAd ad, LoadAdError error) => 
        {
            if(error != null || ad == null)
            {
                print("Rewarded failed to load !! " + error);
                return;
            }
            print("Rewarded ad loaded !!");
            rewardedAd = ad;
            RewardedAdEvents(rewardedAd);
        });
    }
    public void ShowRewardedAd()
    {
        if(rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                print("Give Rewards to Player !!");
                GrantReward(100);
            });
        }
        else
        {
            print("Rewarded ad not Ready !!");
        }
    }
    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}."+
                adValue.Value+
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

#endregion

#region Native

    

    public void RequestNativeAd()
    {
        AdLoader adLoader = new AdLoader.Builder(nat).ForNativeAd().Build();
        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;

        adLoader.LoadAd(new AdRequest());
    }

    private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e)
    {
        print("Native ad loaded !!");
        this.nativeAd = e.nativeAd;
        Texture2D iconTexture = this.nativeAd.GetIconTexture();
        Sprite sprite = Sprite.Create(iconTexture,new Rect(0,0,iconTexture.width,iconTexture.height),Vector2.one * .5f);

        img.sprite = sprite;
    }
    
    private void HandleNativeAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        print("Native ad failed to Load!! " + e.ToString());
    }


#endregion

#region extra coins reward
    void GrantReward(int rewardValue)
    {
        print(rewardValue + " is called");
        totalCoins.text = rewardValue.ToString();
    }
#endregion

}
