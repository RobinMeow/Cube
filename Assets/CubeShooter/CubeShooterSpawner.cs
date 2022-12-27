using RibynsModules;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class CubeShooterSpawner : MonoBehaviour
{
    [SerializeField] CubeShooterRuntimeSet _deadCubeShooters = null;
    [SerializeField] float _respawnDuration = 1.0f;
    [SerializeField] float _invicibilityDuration = 2.5f;
    [SerializeField] Vector3 _respawnLocation = Vector3.zero;
    
    WaitForSeconds WAIT_RESPAWN_DURATION = null;
    WaitForSeconds WAIT_INVINCIBILITY_DURATION = null;

    [RuntimeInitializeOnLoadMethod]
    static void InstantiatePrefab()
    {
        const string prefabFileName = nameof(CubeShooterSpawner);
        GameObject prefab = Resources.Load<GameObject>(nameof(CubeShooterSpawner));
        GameObject cubeShooterSpawner = Instantiate(prefab);
        cubeShooterSpawner.name = prefabFileName;
        DontDestroyOnLoad(cubeShooterSpawner);
    }

    void Awake()
    {
        Assert.IsNotNull(_deadCubeShooters, $"{nameof(_deadCubeShooters)} may not be null.");
        WAIT_RESPAWN_DURATION = new WaitForSeconds(_respawnDuration);
        WAIT_INVINCIBILITY_DURATION = new WaitForSeconds(_invicibilityDuration);
    }

    void Update()
    {
        if (_deadCubeShooters.Count > 0)
        {
            for (int i = 0; i < _deadCubeShooters.Count; i++)
            {
                CubeShooter cubeShooter = _deadCubeShooters[i];
                _deadCubeShooters.Deregister(cubeShooter);
                IEnumerator respawnCubeShooter = cubeShooter.Respawn(WAIT_RESPAWN_DURATION, WAIT_INVINCIBILITY_DURATION, _respawnLocation);
                StartCoroutine(respawnCubeShooter);
            }
        }
    }
}
