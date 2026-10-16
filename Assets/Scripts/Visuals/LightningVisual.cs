using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer))]
public class LightningVisual : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.2f;
    [SerializeField] private float width = 0.12f;
    [SerializeField] private int segments = 12;
    [SerializeField] private float jaggedness = 0.4f;

    private LineRenderer line;
    private float remainingLifetime;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = true;
        line.startWidth = width;
        line.endWidth = width;
    }

    public void Play(Vector3 start, Vector3 end)
    {
        Vector3 direction = (end - start).normalized;
        
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;

        line.positionCount = segments + 1;

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;

            Vector3 position = Vector3.Lerp(start, end, t);

            if (i > 0 && i < segments)
            {
                float offset = Random.Range(-jaggedness, jaggedness);
                position += perpendicular * offset;
            }

            line.SetPosition(i, position);
        }

        remainingLifetime = lifetime;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        remainingLifetime -= Time.deltaTime;
        
        if (remainingLifetime <= 0) 
            gameObject.SetActive(false);
    }
}
