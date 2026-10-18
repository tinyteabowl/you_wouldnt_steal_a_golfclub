using UnityEngine;
using System.Collections;

public class GolfBallTeleport : MonoBehaviour
{
    [SerializeField] private Transform xrOrigin;
    [SerializeField] private Transform headCamera;
    [SerializeField] private GolfBallPhysics ballPhysics;
    [SerializeField] private VRScreenFade screenFade;
    [SerializeField] private Collider clubHeadCollider;
    [SerializeField] private Collider clubHandleCollider;
    [SerializeField] private float distanceFromBall = 1.5f;
    [SerializeField] private float collisionProtectionSeconds = 2f;
    [SerializeField] private float safeDistanceFromBall = 0.3f;

    private bool hasTeleported = false;

    private void Update()
    {
        if (hasTeleported || ballPhysics == null)
            return;

        if (ballPhysics.HasBeenHit && ballPhysics.HasLanded)
        {
            hasTeleported = true;
            StartCoroutine(TeleportAfterLanding());
        }
    }

    private IEnumerator TeleportAfterLanding()
    {
        if (screenFade != null)
            yield return StartCoroutine(screenFade.FadeOut());

        IgnoreClubCollisions(true);

        yield return new WaitForEndOfFrame();

        if (xrOrigin == null || headCamera == null || ballPhysics == null)
        {
            IgnoreClubCollisions(false);

            if (screenFade != null)
                yield return StartCoroutine(screenFade.FadeIn());

            yield break;
        }

        Vector3 direction =
            xrOrigin.position - ballPhysics.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
        {
            direction = -headCamera.forward;
            direction.y = 0f;
        }

        if (direction.sqrMagnitude < 0.01f)
            direction = Vector3.back;

        direction.Normalize();

        Vector3 target =
            ballPhysics.transform.position +
            direction * distanceFromBall;

        if (Physics.Raycast(
            target + Vector3.up * 2f,
            Vector3.down,
            out RaycastHit hit,
            10f))
        {
            Vector3 movement = new Vector3(
                target.x - headCamera.position.x,
                0f,
                target.z - headCamera.position.z
            );

            xrOrigin.position += movement;

            xrOrigin.Rotate(0f, 90f, 0f);
        }
        else
        {
            Debug.LogWarning("No ground found near the ball :<");
        }

        yield return new WaitForSeconds(collisionProtectionSeconds);

        if (screenFade != null)
            yield return StartCoroutine(screenFade.FadeIn());

        while (IsClubNearBall())
            yield return null;

        IgnoreClubCollisions(false);
    }

    private void IgnoreClubCollisions(bool ignore)
    {
        if (ballPhysics == null)
            return;

        Collider ballCollider =
            ballPhysics.GetComponent<Collider>();

        if (ballCollider == null)
            return;

        if (clubHeadCollider != null)
        {
            Physics.IgnoreCollision(
                ballCollider,
                clubHeadCollider,
                ignore
            );
        }

        if (clubHandleCollider != null)
        {
            Physics.IgnoreCollision(
                ballCollider,
                clubHandleCollider,
                ignore
            );
        }
    }

    private bool IsClubNearBall()
    {
        if (ballPhysics == null)
            return false;

        Vector3 ballPosition = ballPhysics.transform.position;

        if (clubHeadCollider != null &&
            Vector3.Distance(
                clubHeadCollider.ClosestPoint(ballPosition),
                ballPosition
            ) < safeDistanceFromBall)
        {
            return true;
        }

        if (clubHandleCollider != null &&
            Vector3.Distance(
                clubHandleCollider.ClosestPoint(ballPosition),
                ballPosition
            ) < safeDistanceFromBall)
        {
            return true;
        }

        return false;
    }

    public void ResetTeleport()
    {
        hasTeleported = false;
    }
}