using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
public class FirebaseManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    Firebase.FirebaseApp app;
    public bool isReadyToUse;

    private void Awake()
    {
        isReadyToUse = false;
    }

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = Firebase.FirebaseApp.DefaultInstance;
                isReadyToUse = true;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    IEnumerator Register()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync("witek.gawlowski@gmail.com", "wefcsdSADFf234ds!?");
        yield return new WaitUntil(() => registerTask.IsCompleted);
        if(registerTask.Exception  != null)
        {
            Debug.Log(registerTask.Exception.ToString() + " exception!");
        }
        else
        {
            print("everything is fine");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("space down");
            StartCoroutine(Register());
        }
    }

}
