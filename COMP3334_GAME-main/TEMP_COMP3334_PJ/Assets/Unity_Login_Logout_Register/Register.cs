using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    public string RegUrl = "http://localhost/3334/reg.php";
    public Text Info;
    public InputField Username_InputField;
    public InputField Password_InputField;
    public InputField ConfirmPassword_InputField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AccountRegister()
    {
        string uName = Username_InputField.text;
        string pWord = Password_InputField.text;
        string CpWord = ConfirmPassword_InputField.text;
        StartCoroutine(RegisterNewAccount(uName, pWord, CpWord));
    }

    IEnumerator RegisterNewAccount(string uName, string pWord, string CpWord)
    {
        WWWForm form = new WWWForm();
        form.AddField("newAccountUsername", uName);
        form.AddField("newAccountPassword", pWord);
        form.AddField("ConfirmPassword", CpWord);
        using (UnityWebRequest www = UnityWebRequest.Post(RegUrl, form))
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
            }
        }
    }

    public void BackLogin()
    {
        SceneManager.LoadSceneAsync("Unity_Login");
    }
}
