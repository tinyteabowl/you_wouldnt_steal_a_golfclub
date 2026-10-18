using System.Collections;
using UnityEngine;

public class PlayerResetOnTouch : MonoBehaviour
{
    public Transform player;
    public Transform golfBall;
    public Transform golfClub;
    public VRScreenFade screenFade;

    private bool resetting = false;

    private Vector3 playerPosition = new Vector3(
        -565.929993f, 5.69999981f, -282.670013f
    );

    private Vector3 golfBallPosition = new Vector3(
        -565.995972f, 6.23000002f, -285.946014f
    );

    private Vector3 golfClubPosition = new Vector3(
        -566.359985f, 6.75f, -285.040009f
    );

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Security trigger touched by: " + other.name);

        bool isPlayer =
            other.CompareTag("Player") ||
            other.GetComponent<CharacterController>() != null ||
            other.GetComponentInParent<CharacterController>() != null;

        if (isPlayer && !resetting)
        {
            StartCoroutine(ResetObjects());
        }
    }


    private IEnumerator ResetObjects()
    {
        resetting = true;

        if (screenFade != null)
            yield return screenFade.FadeOut();

        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.position = playerPosition;
        golfBall.position = golfBallPosition;
        golfClub.position = golfClubPosition;

        Rigidbody ballRigidbody = golfBall.GetComponent<Rigidbody>();

        if (ballRigidbody != null)
        {
            ballRigidbody.linearVelocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
        }

        if (controller != null)
            controller.enabled = true;

        if (screenFade != null)
            yield return screenFade.FadeIn();

        resetting = false;
    }
}