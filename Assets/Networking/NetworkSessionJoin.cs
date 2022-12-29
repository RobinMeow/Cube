using Unity.Netcode;
using UnityEngine;

public class NetworkSessionJoin : MonoBehaviour
{
    [SerializeField] BaseInputs _inputs = null;
    [SerializeField] NetworkManager _networkManager = null;

    void Update()
    {
        if (_inputs.JumpIsPressed && !_inputs.JumpWasPressedPreviousFixedUpdate)
        {
            _networkManager.StartClient();
            
            Debug.Log($"Start Client");
        }
    }
}
