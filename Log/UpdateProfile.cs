using Firebase;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateProfile : MonoBehaviour
{
    
    public TMP_InputField namePlayer;

    public void Updating()
    {
        StartCoroutine(UpdateProfileUser(namePlayer.text));
    }

    public IEnumerator UpdateProfileUser(string name)
    {
        if (namePlayer.text == "")
        {
            Debug.Log("Name is null");
        }
        else
        {
            UserProfile userProfile = new(){ DisplayName = name };
            var taskUpdate = Account.Instance.user.UpdateUserProfileAsync(userProfile);

            yield return new WaitUntil(() => taskUpdate.IsCompleted);

            if (taskUpdate.Exception != null)
            {
                Debug.Log(taskUpdate.Exception);

                FirebaseException firebaseException = taskUpdate.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failsMessage;
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failsMessage = "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failsMessage = "Password is wrong";
                        break;
                    case AuthError.MissingEmail:
                        failsMessage = "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failsMessage = "Password is missing";
                        break;
                    default:
                        failsMessage = "Update is fails";
                        break;
                }

                Debug.Log(failsMessage);
            }
            else
            {
                Debug.Log("Update is Successful");
                LoadScene.Instance.ContinueLogin();
            } 
        }
    }
}
