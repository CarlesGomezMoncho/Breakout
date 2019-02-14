using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    public float itemSpeed = 1.5f;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - itemSpeed * Time.deltaTime, transform.position.z);
    }

    public void DestroyItem()
    {
        AudioSource s;
        s = GetComponents<AudioSource>()[0];
        s.Play();

        Destroy(gameObject, 0.188f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController p = collision.gameObject.GetComponent<PlayerController>();

            if (gameObject.tag == "AgrandarItem")
            {
                p.IncreaseScale(new Vector2(0.5f, 0));  //incrementamos en 1 el ancho de la raqueta

                GetComponent<SpriteRenderer>().sprite = null;   //Quitamos el sprite para que deje de verse inmediatamente (por que tardará algo de tiempo en destruirse el objeto para que suene el sonido)

                DestroyItem();  //destruye el item al contactar con la raqueta
            }

            
        }
    }

}
