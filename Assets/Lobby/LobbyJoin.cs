using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class LobbyJoin : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField _ifUsername;
    
    void Awake() 
    {
        Assert.IsNotNull(_ifUsername, $"{nameof(_ifUsername)} may not be null.");
    }
    
    static bool IsInvalidUsername(string username, out string errorMessage)
    {
        // as if I validate that properly lol
        errorMessage = string.Empty;
        
        if (username == null || string.IsNullOrWhiteSpace(username))
            errorMessage = "Username may not be null, or empty";
        
        if (username.Length > 20)
            errorMessage = "Username too long";
            
        return !string.IsNullOrWhiteSpace(errorMessage);
    }
    
    public void OnHostClick()
    {
        string userInput = _ifUsername.text;
        if (IsInvalidUsername(userInput, out string errorMessage))
        {
            _ifUsername.text = errorMessage;
            return;
        }
        
        NetworkManager.Singleton.StartHost();
    }
    
    public void OnClientClick()
    {
        string userInput = _ifUsername.text;
        if (IsInvalidUsername(userInput, out string errorMessage))
        {
            _ifUsername.text = errorMessage;
            return;
        }
        
        NetworkManager.Singleton.StartClient();
    }
}
