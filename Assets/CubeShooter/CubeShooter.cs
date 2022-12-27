using RibynsModules;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class CubeShooter : MonoBehaviour
{
    [SerializeField] CubeShooterRuntimeSet _deadCubeShooters = null;
    [SerializeField] Renderer _cubeRenderer = null;
    [SerializeField] float _alphaOnInvincibility = 0.5f;
    [SerializeField] Color _color = Color.grey;

    bool _isInvincible = false;
    Color _invincibleColor = default;
    Color _vulnerableColor = default;

    void Awake()
    {
        Assert.IsNotNull(_deadCubeShooters, $"{nameof(_deadCubeShooters)} may not be null.");
        Assert.IsNotNull(_cubeRenderer, $"{nameof(_cubeRenderer)} may not be null.");
        SetColors();
        RegisterAsDead();
    }

    void SetColors()
    {
        _vulnerableColor = _color;
        _cubeRenderer.material.color = _color;
        Color invincColor = _color;
        invincColor.a = _alphaOnInvincibility;
        _invincibleColor = invincColor;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Projectile projectile))
        {
            HitBy(projectile);
        }
    }

    void HitBy(Projectile projectile)
    {
        if (_isInvincible)
        {
            this.Log($"Hit by {projectile.name} registered, but {name} is invincible.");
            return;
        }

        projectile.ReturnToPool();
        // ToDo: play hit effect
        
        SelfDestroy();
    }

    private void SelfDestroy()
    {
        // ToDo: do some particle stuff
        RegisterAsDead();
    }

    void RegisterAsDead()
    {
        _deadCubeShooters.Register(this);
        gameObject.SetActive(false);
    }

    public void RespawnAt(Vector2 worldPosition, WaitForSeconds waitForInvincibilityDuration)
    {
        transform.position = worldPosition;
        MakeInvincible();

        gameObject.SetActive(true);
        StartCoroutine(EndInvincibility(waitForInvincibilityDuration));
    }

    void MakeInvincible()
    {
        _isInvincible = true;
        _cubeRenderer.material.color = _invincibleColor;
    }

    IEnumerator EndInvincibility(WaitForSeconds waitForInvincibilityDuration)
    {
        yield return waitForInvincibilityDuration;
        SetVulnerable();
    }

    void SetVulnerable()
    {
        _isInvincible = false;
        _cubeRenderer.material.color = _vulnerableColor;
    }

    public IEnumerator Respawn(WaitForSeconds waitRespawnDuration, WaitForSeconds waitForVulnerability, Vector2 respawnLocation)
    {
        yield return waitRespawnDuration;
        RespawnAt(respawnLocation, waitForVulnerability);
    }
}
