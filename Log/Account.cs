using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;
using UnityEngine.UI;
using System.Xml.Serialization;
using Firebase;
using Firebase.Auth;
using Unity.Notifications.iOS;
using System.Data;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class Account : MonoBehaviour
{
    public static Account Instance;

    private DependencyStatus status;
    protected FirebaseAuth auth;
    public FirebaseUser user;

    [Header("Field Login")]
    [SerializeField]private TMP_InputField email_Login;
    [SerializeField] private TMP_InputField password_Login;

    [Space]
    [Header("Field SignUp")]
    [SerializeField] private TMP_InputField email_SignUp;
    [SerializeField] private TMP_InputField password_SignUp;
    [SerializeField] private TMP_InputField confirm_Password;
    

    private void CreateAccount()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    //Check dependencies for firebase
    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            status = task.Result;
            if (status == DependencyStatus.Available)
            {
                InitializedFirebase();
            }
            else
            {
                Debug.Log("Denpendencies unavailable with state : " + status);
            }
        });

        CreateAccount();
    }

   void StatusChanged(object sender, System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn &&  user != null)
            {
                Debug.Log("signed out " + user.UserId);
            }

            user = auth.CurrentUser;

            if(signedIn)
            {
                Debug.Log("signed in " + user.UserId);
                
            }
        }
    }

    public void InitializedFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += StatusChanged;
        StatusChanged(this, null);
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(email_Login.text, password_Login.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if(loginTask.Exception != null)
        {
            Debug.Log("Error : " +  loginTask.Exception);
            
            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError) firebaseException.ErrorCode;

            string errorMessage = "Couldn't log in because ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    errorMessage += "email is invalid";
                    break;
                case AuthError.WrongPassword:
                    errorMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    errorMessage += "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    errorMessage += "Missing Password";
                    break;
                default:
                    errorMessage = "Login failed";
                    break;
            }

            Debug.Log(errorMessage);
        }
        else
        {
            user = loginTask.Result.User;

            Debug.LogFormat("{0} You are successfully log in", user.DisplayName);

            SceneManager.LoadSceneAsync(1);
        }
    }

    public void SignUp()
    {
        StartCoroutine(SignUpAccount(email_SignUp.text, password_SignUp.text, confirm_Password.text));
    }
    private IEnumerator SignUpAccount(string email, string password, string confirmPassword)
    {
        if (email == "")
        {
            Debug.LogError("Email field is empty");
        }
        else if (password == null)
        {
            Debug.LogError("Password is empty");
        }
        else if (password != confirmPassword)
        {
            Debug.LogError("Password is not match");
        }
        else
        {
            var signUpTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => signUpTask.IsCompleted);

            if (signUpTask.Exception != null)
            {
                Debug.Log(signUpTask.Exception);

                FirebaseException firebaseException = signUpTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failSignUpMessage = "Couldn't Sign Up Because ";

                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failSignUpMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failSignUpMessage += "Password is wrong";
                        break;
                    case AuthError.MissingEmail:
                        failSignUpMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failSignUpMessage += "Password is missing";
                        break;
                    default:
                        failSignUpMessage = "Sign Up fail";
                        break;
                }

                Debug.LogError(failSignUpMessage);
            }
            else
            {
                user = signUpTask.Result.User;
                Debug.Log("Successfully");
                PanelLogOnAndOff.Instance.ClosePanel();
            }
        }
    }
}
