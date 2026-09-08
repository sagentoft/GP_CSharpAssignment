using UnityEngine;

public class Health : MonoBehaviour, IDamageable<int>, IKillable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int minHealth;
    [SerializeField] private int currentHealth;

    private int defaultHealth = 50;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = SetDefaultHealth(maxHealth);
        currentHealth = maxHealth;
    }

    private int SetDefaultHealth(int currentMaxVal)
    {
        if (currentMaxVal > 0)
        {
            return currentMaxVal;
        }
        else
        {
            return defaultHealth;
        }
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= minHealth)
        {
            Kill();
        }
    }

    public void Kill()
    {
        throw new System.NotImplementedException();
    }
}
