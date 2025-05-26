using UnityEngine;

public class ArrowFlight : MonoBehaviour
{
    private Rigidbody rb;
    public TrailRenderer trail;
    public float trailDelay = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (trail != null)
        {
            trail.emitting = false; // Turn off trail initially
            Invoke(nameof(EnableTrail), trailDelay);
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // Rotate arrow to face velocity direction if moving fast enough
        if (rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            transform.forward = rb.linearVelocity.normalized;
        }
    }

    void EnableTrail()
    {
        if (trail != null)
            trail.emitting = true;
    }
}
