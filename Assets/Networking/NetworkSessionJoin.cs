using RibynsModules.GameLogger;
using Unity.Netcode;
using UnityEngine;

public class NetworkSessionJoin : MonoBehaviour
{
    [SerializeField] NetworkManager _networkManager = null;
    GameLogger _logger = null;
    bool _joinedSession = false;

    void Awake()
    {
        _logger = this.CreateLogger("NetworkSessionJoin");
    }

    void OnGUI()
    {
        Vector2 middleTop = new Vector2(Screen.width / 2.0f, 0.0f);
        Vector2 size = new Vector2(200.0f, 200.0f);
        Rect top = new Rect(middleTop, size);
        
        Rect middle = new Rect(middleTop + new Vector2(0.0f, size.y), size);
        Rect bottom = new Rect(middleTop + new Vector2(0.0f, 2 * size.y), size);

        if (!_joinedSession)
        {
            if (GUI.Button(top, "Host"))
            {
                _joinedSession = _networkManager.StartHost();
                if (_joinedSession)
                    _logger.Log($"Joined as Host");
                else
                    _logger.Log($"Join as Host. Failed!");
            }
            else if (GUI.Button(middle, "Client"))
            {
                _joinedSession = _networkManager.StartClient();
                if (_joinedSession)
                    _logger.Log($"Joined as Client");
                else
                    _logger.Log($"Join as Client. Failed!");
            }
            else if (GUI.Button(bottom, "Server"))
            {
                _joinedSession = _networkManager.StartServer();
                if (_joinedSession)
                    _logger.Log($"Joined as Server");
                else
                    _logger.Log($"Join as Server. Failed!");
            }
        }
    }
}
