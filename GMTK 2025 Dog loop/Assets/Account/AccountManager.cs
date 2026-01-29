using Unity.Services.Authentication.PlayerAccounts;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine.UI;
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
            CurrentUsername = Username.text;
            CurrentPassword = Password.text;
            OnAccountSignedIn.Raise();
            await SceneManager.UnloadSceneAsync("AccountSignIn");
            return;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(Username.text, Password.text);
                CurrentUsername = Username.text;
                CurrentPassword = Password.text;
                OnAccountSignedIn.Raise();
                await SceneManager.UnloadSceneAsync("AccountSignIn");
                return;
            }
            catch (RequestFailedException ex2)
            {
                Debug.LogException(ex2);
            }
        }

    }

    async void SignInWithUnity() 
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            //ExternalIds = GetExternalIds(AuthenticationService.Instance.PlayerInfo);
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
