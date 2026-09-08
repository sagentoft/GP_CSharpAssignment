using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IKillable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<IDamageable<int>>().Damage(4);
        }
    }
}
