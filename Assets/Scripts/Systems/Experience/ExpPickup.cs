using System;
using Unity.VisualScripting;
using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    [SerializeField] private int experienceValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        Experience experience = other.GetComponent<Experience>();

        if (experience != null)
        {
            experience.AddExperience(experienceValue);
            gameObject.SetActive(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
