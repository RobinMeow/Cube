using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class LobbyJoin : MonoBehaviour
{
    [SerializeField] NetworkManager _networkManager;
    [SerializeField] GameObject _userInterface;
    [SerializeField] TMPro.TMP_InputField _ifUsername;
    
    void Awake() 
    {
        Assert.IsNotNull(_ifUsername, $"{nameof(_ifUsername)} may not be null.");
        Assert.IsNotNull(_userInterface, $"{nameof(_userInterface)} may not be null.");
    }
    
    public void OnHostClick()
    {
        string userInput = _ifUsername.text;
        if (IsInvalidUsername(userInput, out string errorMessage))
        {
            _ifUsername.text = errorMessage;
            return;
        }
        
        _networkManager.StartHost();
        _userInterface.SetActive(false); 
    }
    
    public void OnClientClick()
    {
        string userInput = _ifUsername.text;
        if (IsInvalidUsername(userInput, out string errorMessage))
        {
            _ifUsername.text = errorMessage;
            return;
        }
        
        _networkManager.StartClient();
        _userInterface.SetActive(false);
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
}
