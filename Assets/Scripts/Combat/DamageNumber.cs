using System;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float lifeTime = 0.75f;
    [SerializeField] private float moveSpeed = 1f;
    
    private float elapsedTime;
    private Camera mainCamera;
    private Color originalColor;

    private void Awake()
    {
        mainCamera = Camera.main;
        originalColor = text.color;
    }

    public void Show(int damage, Color color)
    {
        text.text = damage.ToString();
        originalColor = color;
        text.color = originalColor;
        elapsedTime = 0f;
    }
    
    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        transform.position += Vector3.up * (moveSpeed * Time.deltaTime);
        
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        
        float progress = elapsedTime / lifeTime;
        float fadeProgress = Mathf.InverseLerp(0.5f, 1f, progress);

        Color color = originalColor;
        color.a = Mathf.Lerp(originalColor.a, 0f, fadeProgress);
        text.color = color;

        if (elapsedTime >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }
}
