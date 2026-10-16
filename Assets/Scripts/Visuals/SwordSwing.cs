using System;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    [SerializeField] private float swingAngle = 120f;
    [SerializeField] private float swingDuration = 0.2f;
    [SerializeField] private float baseRange = 2f;
    [SerializeField] private Transform swingVisual;

    private Vector3 baseScale;
    private Quaternion startingRotation;
    private float elapsedTime;
    private bool reverse;

    private void Awake()
    {
        baseScale = swingVisual.localScale;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
    }
    
    public void Play(Vector3 direction, float range, bool reverse)
    {
        elapsedTime = 0f;
        this.reverse = reverse;

        transform.rotation =
            Quaternion.LookRotation(direction);

        startingRotation = transform.rotation;

        float rangeMultiplier = range / baseRange;
        
        swingVisual.localScale = new Vector3(baseScale.x, baseScale.y, baseScale.z * rangeMultiplier);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        float progress = Mathf.Clamp01(elapsedTime / swingDuration);
        
        float startAngle = reverse ? swingAngle / 2f : -swingAngle / 2f;

        float endAngle = reverse ? -swingAngle / 2f : swingAngle / 2f;
        
        float angle = Mathf.Lerp(startAngle, endAngle, progress);
        
        transform.rotation = startingRotation * Quaternion.Euler(0f, angle, 0f);

        if (progress >= 1f)
        {
            gameObject.SetActive(false);
        }
    }
}
