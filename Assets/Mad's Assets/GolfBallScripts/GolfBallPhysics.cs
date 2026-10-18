using UnityEngine;

public class GolfBallPhysics : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasLanded = false;
    private bool hasBeenHit = false;
    private RigidbodyConstraints normalConstraints;
    public bool HasLanded => hasLanded;
    public bool HasBeenHit => hasBeenHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        normalConstraints = rb.constraints;
    }

    public void PrepareForShot()
    {
        hasBeenHit = true;
        hasLanded = false;

        rb.isKinematic = false;
        rb.constraints = normalConstraints;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasBeenHit && !hasLanded &&
            collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}