using UnityEngine;

public class BowEvents : MonoBehaviour
{
    public float arrowSpeed = 30f;
    public GameObject arrowVisual;
    public FirstPersonController playerController;

    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;

    public void HideArrow()
    {
        if (arrowVisual != null)
            arrowVisual.SetActive(false);
    }

    public void ShowArrow()
    {
        if (arrowVisual != null)
            arrowVisual.SetActive(true);
    }
    public void ShootArrow()
    {
        // Correction rotation (adjust as needed)
        Quaternion correctionRotation = Quaternion.Euler(0f, 0f, 90f);

        // Instantiate arrow with corrected rotation
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation * correctionRotation);

        // Hide arrow visuals initially
        Renderer[] renderers = arrow.GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
            r.enabled = false;

        // Fix direction (already done via rotation above)
        // You could do other adjustments here if needed

        // Show arrow visuals after correction
        foreach (var r in renderers)
            r.enabled = true;

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 playerVelocity = playerController != null ? playerController.Velocity : Vector3.zero;
            rb.linearVelocity = arrowSpawnPoint.forward * arrowSpeed + playerVelocity;
        }

        Destroy(arrow, 5f);
    }

}