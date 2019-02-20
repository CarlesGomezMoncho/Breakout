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
        PlayerController p = collision.gameObject.GetComponent<PlayerController>();

        //si choca con un objeto que tenga este componente (si choca con la raqueta)
        if (p)
        {
            if (gameObject.tag == "AgrandarItem")
            {
                //incrementamos en 1 el ancho de la raqueta
                p.IncreaseScale(new Vector2(0.5f, 0));  
            }
            else if (gameObject.tag == "3BolesItem" )//&& GameController.instance.)
            {
                GameObject newBall;
                GameObject lastBall = GameController.instance.GetRandomBall();

                if (lastBall)
                {
                    Vector2 firstBallPosition = lastBall.transform.position;

                    //creamos 2 bolas mas
                    newBall = GameController.instance.AddPelota();
                    newBall.transform.position = new Vector2(firstBallPosition.x - (newBall.GetComponent<SpriteRenderer>().sprite.bounds.size.x * 2), firstBallPosition.y);
                    newBall.GetComponent<BallController>().SetImpulsa(true);
                    newBall.GetComponent<BallController>().SetIniciada(true);
                    newBall.GetComponent<BallController>().SetDirectionToCopy(lastBall.GetComponent<Rigidbody2D>().velocity);

                    newBall = GameController.instance.AddPelota();
                    newBall.transform.position = new Vector2(firstBallPosition.x + (newBall.GetComponent<SpriteRenderer>().sprite.bounds.size.x * 2), firstBallPosition.y);
                    newBall.GetComponent<BallController>().SetImpulsa(true);
                    newBall.GetComponent<BallController>().SetIniciada(true);
                    newBall.GetComponent<BallController>().SetDirectionToCopy(lastBall.GetComponent<Rigidbody2D>().velocity);
                }
            }

            GetComponent<SpriteRenderer>().sprite = null;   //Quitamos el sprite para que deje de verse inmediatamente (por que tardará algo de tiempo en destruirse el objeto para que suene el sonido)
            DestroyItem();  //destruye el item al contactar con la raqueta



        }
    }

}
