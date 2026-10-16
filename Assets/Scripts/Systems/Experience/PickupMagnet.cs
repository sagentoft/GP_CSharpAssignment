using UnityEngine;

public class PickupMagnet : MonoBehaviour
{
    [SerializeField] private float pickupRadius = 2f;
    [SerializeField] private LayerMask pickupLayer;

    private readonly Collider[] nearbyPickups = new Collider[64];

    // Update is called once per frame
    void Update()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, pickupRadius, nearbyPickups,
            pickupLayer);

        for (int i = 0; i < count; i++)
        {
            if (nearbyPickups[i].TryGetComponent(out ExpPickup pickup))
            {
                pickup.Attract(transform);
            }
        }
    }

    public void ModifyPickupRadius(float multiplier)
    {
        pickupRadius *= multiplier;
    }
}
