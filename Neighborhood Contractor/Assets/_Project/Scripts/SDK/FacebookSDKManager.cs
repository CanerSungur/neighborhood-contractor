using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

/// <summary>
/// You can check website for more info:
/// https://assist-software.net/snippets/how-integrate-facebook-api-unity-3d#ii-unity-d-configuration
/// </summary>
public class FacebookSDKManager : Singleton<FacebookSDKManager>
{
    private void Awake()
    {
        this.Reload();

        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    #region Login & Logout Functions

    // Button variables
    //public Button btnLogin, btnLogout, btnName;

    //public void FacebookLogin()
    //{
    //    var permissions = new List<string>() { "public_profile", "email", "user_friends" };
    //    FB.LogInWithReadPermissions(permissions, AuthCallback);
    //}

    //private void AuthCallback(ILoginResult result)
    //{
    //    if (FB.IsLoggedIn)
    //    {
    //        // AccessToken class will have session details
    //        var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
    //        // Print current access token's User ID
    //        Debug.Log(aToken.UserId);
    //        // Print current access token's granted permissions
    //        foreach (string perm in aToken.Permissions)
    //            Debug.Log(perm);
    //    }
    //    else
    //    {
    //        Debug.Log("User cancelled login");
    //    }
    //}
    //public void FacebookLogout()
    //{
    //    FB.LogOut();
    //}

    //// For profile name use this function
    //public void GetName()
    //{
    //    FB.API("me?fields=name", Facebook.Unity.HttpMethod.GET, delegate (IGraphResult result)
    //    {
    //        if (result.ResultDictionary != null)
    //        {
    //            foreach (string key in result.ResultDictionary.Keys)
    //            {
    //                Debug.Log(key + " : " + result.ResultDictionary[key].ToString());
    //                if (key == "name")
    //                    btnName.GetComponentInChildren<Text>().text = result.ResultDictionary[key].ToString();
    //            }
    //        }
    //    });
    //}

    //// For Buttons
    //void Start()
    //{
    //    btnLogin.onClick.AddListener(() => FacebookLogin());
    //    btnLogout.onClick.AddListener(() => FacebookLogout());
    //    btnName.onClick.AddListener(() => GetName());
    //}

    #endregion
}
