using System.Collections;
using UnityEngine;

public class BallDestroyer : MonoBehaviour
{

    private AudioSource liveLostSound;

    private void Start()
    {
        liveLostSound = GetComponents<AudioSource>()[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pelota"))
        {
            StartCoroutine(DestroyBall(collision.gameObject, 1.5f));
        }
    }

    private IEnumerator DestroyBall(GameObject ball, float seconds = 0)
    {
        if (GameController.instance.TileCount() > 0)
        {
            liveLostSound.Play();
        }
        yield return new WaitForSeconds(seconds);
        GameController.instance.RemovePelota(ball);
    }
}
