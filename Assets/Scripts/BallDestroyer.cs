using System.Collections;
using UnityEngine;

public class BallDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pelota"))
        {
            StartCoroutine(DestroyBall(collision.gameObject, 2));
        }
    }

    private IEnumerator DestroyBall(GameObject ball, int seconds = 0)
    {
        yield return new WaitForSeconds(seconds);
        GameController.instance.RemovePelota(ball);
    }
}
