using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloatingVisual : MonoBehaviour
{

    [Header("Bobbing")] 
    [SerializeField] private float bobHeight = 0.1f;
    [SerializeField] private float bobSpeed = 2f;

    [Header("Rotation")] 
    [SerializeField] private float rotationSpeed = 60f;

    private Vector3 initialLocalPosition;
    private float elapsedTime;

    private void Awake()
    {
        initialLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        elapsedTime = Random.Range(0f, Mathf.PI * 2f);
        transform.localPosition = initialLocalPosition;
    }
    
    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        float offset = Mathf.Sin(elapsedTime * bobSpeed) * bobHeight;
        
        transform.localPosition = initialLocalPosition + Vector3.up * offset;
        
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
