using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobManagerScript : MonoBehaviour
{

    private BannerView bannerView;
    void Start()
    {
        string appID = "ca-app-pub-1069030067751675~2491247106";
        MobileAds.Initialize(appID);
        this.RequestBanner();
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
        Debug.Log("Request Banner End");
    }
}