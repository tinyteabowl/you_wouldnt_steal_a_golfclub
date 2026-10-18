/*
 Set it up:
1. Select a laser or hazard object.
2. Add a Collider if it doesn't already have one.
3. Enable Is Trigger on that collider.
4. Add the HazardReset script to the hazard.
5. Drag your XR Origin into the Xr Origin field.
6. Drag Reset into the Starting Spawn field.
 */
using UnityEngine;

public class Reset : MonoBehaviour
{
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private Transform startingSpawn;

    private void OnTriggerEnter(Collider other)
    {
        // Checks whether the object entering is part of the VR player.
        if (xrOrigin == null || startingSpawn == null)
            return;

        if (other.transform == xrOrigin ||
            other.transform.IsChildOf(xrOrigin))
        {
            // Returns the player to the starting position.
            xrOrigin.position = startingSpawn.position;
        }
    }
}