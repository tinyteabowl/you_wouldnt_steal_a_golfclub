using UnityEngine;

public class GolfClubHit : MonoBehaviour
{
    [SerializeField] private float hitForce = 5f;
    [SerializeField] private float liftForce = 1.5f;
    [SerializeField] private GolfBallTeleport teleportSystem;

    private Vector3 previousPosition;
    private Vector3 clubVelocity;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        clubVelocity =
            (transform.position - previousPosition) / Time.fixedDeltaTime;

        previousPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("GolfBall"))
            return;

        Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
        GolfBallPhysics ballPhysics =
            collision.gameObject.GetComponent<GolfBallPhysics>();

        if (ballRb == null || ballPhysics == null)
            return;

        ballPhysics.PrepareForShot();

        if (teleportSystem != null)
            teleportSystem.ResetTeleport();

        Vector3 hitDirection = clubVelocity;
        hitDirection.y = 0f;

        if (hitDirection.sqrMagnitude < 0.01f)
            hitDirection = transform.forward;

        hitDirection =
            (hitDirection.normalized + Vector3.up * (liftForce / hitForce))
            .normalized;

        ballRb.AddForce(hitDirection * hitForce, ForceMode.Impulse);
    }
}