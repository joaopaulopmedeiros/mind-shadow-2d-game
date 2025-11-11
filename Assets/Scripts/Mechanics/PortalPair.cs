using UnityEngine;

public class PortalPair : MonoBehaviour
{
    public PortalPair connectedPortal;  
    public float teleportCooldown = 0.5f;

    private bool canTeleport = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        RatController rat = other.GetComponent<RatController>();
        if (rat == null) return;

        if (!canTeleport) return;

        StartCoroutine(Teleport(rat));
    }

    private System.Collections.IEnumerator Teleport(RatController rat)
    {
        if (connectedPortal == null)
        {
            Debug.LogWarning($"{name} não possui um portal conectado!");
            yield break;
        }

        canTeleport = false;
        connectedPortal.canTeleport = false;

        rat.transform.position = connectedPortal.transform.position;

        yield return new WaitForSeconds(teleportCooldown);

        canTeleport = true;
        connectedPortal.canTeleport = true;
    }
}
