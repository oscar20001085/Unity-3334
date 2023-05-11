using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using TMPro;
using System.IO;
using System.Text;
using System.Security.Cryptography;


public class Chat : NetworkBehaviour
{
    [SerializeField] private GameObject chatUI = null;
    [SerializeField] private TMP_Text chatText = null;
    [SerializeField] private TMP_InputField inputField = null;

    public static String Username = "Alice";
    public static String Username2 = "Bob";
    public static String Username3 = "Charlie";
    public static String Username4 = "David";
    

    private static event Action<string> OnMessage;

    private ChatLogger logger; // reference to the logger

    public override void OnStartAuthority()
    {
        chatUI.SetActive(true);

        // initialize the logger
        logger = new ChatLogger();
        logger.Initialize();

        OnMessage += HandleNewMessage;
    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!hasAuthority) { return; }

        OnMessage -= HandleNewMessage;

        // finalize the logger
        logger.Finalize();
    }

    private void HandleNewMessage(string message)
    {
        chatText.text += message;

        // log the message
        logger.Log(message);
    }

    [Client]
    public void Send(string message)
    {
        if(!hasAuthority) {
            return;
        }

        if (!Input.GetKeyDown(KeyCode.Return)) { return; }

        if (string.IsNullOrWhiteSpace(message)) { return; }

        CmdSendMessage(message);

        inputField.text = string.Empty;
    }

    [Command]
    private void CmdSendMessage(string message)
    {
        //RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");

        if (connectionToClient.connectionId == 0){
            RpcHandleMessage($"[{Username}]: {message}");
        }
        else if(connectionToClient.connectionId == 1){
            RpcHandleMessage($"[{Username2}]: {message}");
        }
        else if(connectionToClient.connectionId == 2){
            RpcHandleMessage($"[{Username3}]: {message}");
        }
        else {
             RpcHandleMessage($"[{Username4}]: {message}");
        }
    }

    [ClientRpc]
    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }
}

public class ChatLogger
{
    private StreamWriter writer;

    public void Initialize()
    {
        // create a log file with a unique name based on the current date and time
        string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
        writer = new StreamWriter(filename);
    }

    public void Log(string message)
    {
        // write the message to the log file
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        writer.WriteLine($"{timestamp} {message}");
        writer.Flush(); // flush the buffer to ensure the message is written immediately
    }

    public void Finalize()
    {
        // close the log file
        writer.Close();
    }
}