using System;
using Unity.VisualScripting;
using UnityEngine;

public class ExpPickup : MonoBehaviour
{
    [SerializeField] private int experienceValue = 1;
    [SerializeField] private float attractionSpeed = 6f;
    [SerializeField] private float attractionAcceleration = 12f;
    [SerializeField] private AudioClip pickupSound;
    
    [Header("Visuals")]
    [SerializeField] private Renderer pickupRenderer;

    [SerializeField] private Color smallColor = Color.cyan;
    [SerializeField] private Color mediumColor = Color.green;
    [SerializeField] private Color largeColor = Color.magenta;

    [SerializeField] private float mediumScale = 1.25f;
    [SerializeField] private float largeScale = 1.5f;

    private Vector3 originalScale;
    private MaterialPropertyBlock propertyBlock;

    private Transform attractionTarget;
    private float currentAttractionSpeed;

    private void Awake()
    {
        originalScale = transform.localScale;
        propertyBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        attractionTarget = null;
        currentAttractionSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (attractionTarget == null)
            return;
        
        currentAttractionSpeed += attractionAcceleration * Time.deltaTime;
        
        transform.position = Vector3.MoveTowards(transform.position, attractionTarget.position,
            currentAttractionSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Experience experience = other.GetComponent<Experience>();

        if (experience != null)
        {
            experience.AddExperience(experienceValue);
            
            AudioManager.Instance.PlayPickup(pickupSound);
            
            gameObject.SetActive(false);
        }
    }

    public void Attract(Transform target)
    {
        if (attractionTarget != null)
            return;
        
        attractionTarget = target;
        currentAttractionSpeed = attractionSpeed;
    }
    
    public void Initialize(int expAmount)
    {
        experienceValue = expAmount;
        
        experienceValue = expAmount;

        Color color;
        float scale;

        if (expAmount >= 8)
        {
            color = largeColor;
            scale = largeScale;
        }
        else if (expAmount >= 3)
        {
            color = mediumColor;
            scale = mediumScale;
        }
        else
        {
            color = smallColor;
            scale = 1f;
        }

        transform.localScale = originalScale * scale;

        pickupRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_BaseColor", color);
        pickupRenderer.SetPropertyBlock(propertyBlock);
    }
}
