using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using TMPro;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private TMP_InputField Username;
    [SerializeField]
    private TMP_InputField Password;


    private string CurrentUsername;
    private string CurrentPassword;

    [SerializeField]
    private GameEvent OnAccountSignedIn;
    public async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError(ex.Message);
        }
        PlayerAccountService.Instance.SignedIn += SignInWithUnity;
        PlayerAccountService.Instance.SignedIn += SignInPasswordUsername;

        DontDestroyOnLoad(this);

    }

    public async void Start() 
    {
        if (AuthenticationService.Instance.IsSignedIn)
        {
            print("AlreadySignedIn");
            SceneManager.UnloadSceneAsync("AccountSignIn");
            return;
        }
    }

    public async void StartSignIn()
    {
        if (PlayerAccountService.Instance.IsSignedIn)
        {
            SignInWithUnity();
            return;
        }

        try 
        {
            await PlayerAccountService.Instance.StartSignInAsync();
        }
        catch(RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    public async void SignInPasswordUsername() 
    {
        if (Username.text.Contains('#')) return;

        if (PlayerAccountService.Instance.IsSignedIn)
        {

            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(CurrentUsername, CurrentPassword);
                return;
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }


        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(Username.text, Password.text);
            await AuthenticationService.Instance.UpdatePlayerNameAsync(Username.text);
            CurrentUsername = Username.text;
            CurrentPassword = Password.text;
            OnAccountSignedIn.Raise();
          //  await SceneManager.UnloadSceneAsync("AccountSignIn");
            return;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(Username.text, Password.text);
                await AuthenticationService.Instance.UpdatePlayerNameAsync(Username.text);

                CurrentUsername = Username.text;
                CurrentPassword = Password.text;
                OnAccountSignedIn.Raise();
           //     await SceneManager.UnloadSceneAsync("AccountSignIn");
                return;
            }
            catch (RequestFailedException ex2)
            {
                Debug.LogException(ex2);
            }
        }

    }

    public async void LinkWithUnity() 
    {
        try
        {
            if (!PlayerAccountService.Instance.IsSignedIn) { await PlayerAccountService.Instance.StartSignInAsync(); }

            LinkOptions options = new LinkOptions();
            options.ForceLink = true;
           // await AuthenticationService.Instance.A(PlayerAccountService.Instance.AccessToken);
            await AuthenticationService.Instance.LinkWithUnityAsync(PlayerAccountService.Instance.AccessToken,options);
            OnAccountSignedIn.Raise();
            await SceneManager.UnloadSceneAsync("AccountSignIn");
            // ExternalIds = GetExternalIds(AuthenticationService.Instance.PlayerInfo);
        }
        catch (RequestFailedException ex) 
        {
            Debug.LogException(ex);
        }
    }


    public async void SignInWithUnity()
    {
        try
        {
            if (!PlayerAccountService.Instance.IsSignedIn) { await PlayerAccountService.Instance.StartSignInAsync(); }

            LinkOptions options = new LinkOptions();
            options.ForceLink = true;
             await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            //await AuthenticationService.Instance.LinkWithUnityAsync(PlayerAccountService.Instance.AccessToken, options);
            OnAccountSignedIn.Raise();
            await SceneManager.UnloadSceneAsync("AccountSignIn");
            // ExternalIds = GetExternalIds(AuthenticationService.Instance.PlayerInfo);
            // ExternalIds = GetExternalIds(AuthenticationService.Instance.PlayerInfo);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
