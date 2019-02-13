using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public void DestroyItem(PlayerController playerController)
    {
        AudioSource s;
        s = GetComponents<AudioSource>()[0];
        s.Play();

        playerController.IncreaseScale(new Vector3(1, 0, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController p = collision.gameObject.GetComponent<PlayerController>();

            if (gameObject.name == "AgrandarItem")
            {
                p.IncreaseScale(new Vector2(0.5f, 0));  //incrementamos en 1 el ancho de la raqueta
                p.GetSpriteWidth();                     //para reajustar el limite de movimiento
            }

            
        }
    }

}
