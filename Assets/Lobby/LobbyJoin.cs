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
        // if (IsServer)
        //     _networkManager.OnClientConnectedCallback += OnClientConnected;
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
        SpawnCubeShooterServerRpc(); // Host does not count as client, apperently.
    }
    
    public void OnClientClick()
    {
        string userInput = _ifUsername.text;
        if (IsInvalidUsername(userInput, out string errorMessage))
        {
            _ifUsername.text = errorMessage;
            return;
        }
        
        _userInterface.SetActive(false);
        _networkManager.StartClient();
        StartCoroutine(RequestCubeShooterSpawn());
        // SpawnCubeShooterServerRpc(); // Host does not count as client, apperently.
    }

    System.Collections.IEnumerator RequestCubeShooterSpawn()
    {
        yield return new WaitForSeconds(3.0f);
        SpawnCubeShooterServerRpc();
    }

    void OnClientConnected(ulong clientId) 
    {
        SpawnCubeShooterServerRpc();
    }
    
    [ServerRpc(Delivery = RpcDelivery.Reliable, RequireOwnership = false)]
    void SpawnCubeShooterServerRpc(ServerRpcParams para = default)
    {
        ulong senderClientId = para.Receive.SenderClientId;
        Debug.Log("SpawnCubeShooterServerRpc Raised");
        GameObject gameObject = Instantiate(_cubePrefab, Vector3.zero, Quaternion.identity);
        gameObject.name = $"{_ifUsername.text} (CubeShooter)";
        NetworkObject networkObject = gameObject.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(senderClientId, destroyWithScene: true); 
        // position is handled via RuntimeSet and CubeSpawner somehow, cant remember. But pretty cool.
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
    
    public override void OnDestroy() 
    {
        // _networkManager.OnClientConnectedCallback -= OnClientConnected;
        base.OnDestroy();
    }
}
