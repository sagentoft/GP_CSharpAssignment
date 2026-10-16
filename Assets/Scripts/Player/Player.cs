using System;
using UnityEngine;

public class Player : MonoBehaviour, IKillable
{
    [SerializeField] private Health health;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private float deathHeight = -20f;

    private void OnEnable()
    {
        health.OnDamaged += PlayDamageSound;
    }

    private void OnDisable()
    {
        health.OnDamaged -= PlayDamageSound;
    }

    private void Update()
    {
        if (transform.position.y < deathHeight)
        {
            health.Damage(health.MaxHealth);
        }
    }

    private void PlayDamageSound(int damage)
    {
        AudioManager.Instance.PlaySFX(damageSound);
    }

    public void Die()
    {
        GameManager.Instance.GameOver();
    }
}
