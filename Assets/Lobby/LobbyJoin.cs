using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class LobbyJoin : NetworkBehaviour
{
    [SerializeField] NetworkManager _networkManager;
    [SerializeField] TMPro.TMP_InputField _ifUsername;
    [SerializeField] GameObject _userInterface;
    [SerializeField] GameObject _cubePrefab;
    
    void Awake() 
    {
        Assert.IsNotNull(_ifUsername, $"{nameof(_ifUsername)} may not be null.");
        Assert.IsNotNull(_userInterface, $"{nameof(_userInterface)} may not be null.");
        Assert.IsNotNull(_networkManager, $"{nameof(_networkManager)} may not be null.");
        Assert.IsNotNull(_cubePrefab, $"{nameof(_cubePrefab)} may not be null.");
    }
    
    void Start()
    {
        _networkManager.OnClientConnectedCallback += OnClientConnected;
    }
    
    public void OnHostClick()
    {
        string userInput = _ifUsername.text;
        if (IsInvalidUsername(userInput, out string errorMessage))
        {
            _ifUsername.text = errorMessage;
            return;
        }
        
        _userInterface.SetActive(false);
        _networkManager.StartHost();
    }
    
    public void OnClientClick()
    {
        string userInput = _ifUsername.text;
        if (IsInvalidUsername(userInput, out string errorMessage))
        {
            _ifUsername.text = errorMessage;
            return;
        }
        
        _networkManager.OnClientConnectedCallback -= OnClientConnected; // only the host needs it
        _userInterface.SetActive(false);
        _networkManager.StartClient();
    }

    void OnClientConnected(ulong clientId) 
    {
        Debug.Log("OnClientConnected called on " + OwnerClientId);
        GameObject gameObject = Instantiate(_cubePrefab, Vector3.zero, Quaternion.identity);
        gameObject.name = $"{clientId} (CubeShooter)";
        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(clientId, destroyWithScene: true); // ToDo: check if default is server
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
