using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour
{
    public string LoginUrl = "http://localhost/3334/login.php";
    public Text Info;
    public InputField Username_InputField;
    public InputField Password_InputField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AccountLogin()
    {
        string uName = Username_InputField.text;
        string pWord = Password_InputField.text;
        StartCoroutine(LoginAccount(uName, pWord));
    }

    IEnumerator LoginAccount(string uName, string pWord)
    {
        WWWForm form = new WWWForm();
        form.AddField("newAccountUsername", uName);
        form.AddField("newAccountPassword", pWord);
        using (UnityWebRequest www = UnityWebRequest.Post(LoginUrl, form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
  
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Response = " + responseText);
                Info.text = responseText;

                if (responseText == "Login Success"){
                    SceneManager.LoadSceneAsync("main_game");
                }
            }
        }
    }

    public void BackReg()
    {
        SceneManager.LoadSceneAsync("Unity_Register");
    }
}
