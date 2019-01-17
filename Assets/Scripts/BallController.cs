using UnityEngine;
using UnityEngine.Tilemaps;

public class BallController : MonoBehaviour
{
    public Transform centroRaqueta;
    public Tilemap tileMap;

    public float fuerzaInicial = 2;

    private bool iniciada = false;
    private bool impulsa = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!iniciada)
            transform.position = centroRaqueta.position;

    }

    private void FixedUpdate()
    {
        if(impulsa)
        {
            Impulsa(fuerzaInicial);
        }
    }

    public void IniciarMovimiento()
    {
        iniciada = true;
        impulsa = true; //impulso inicial
    }

    //impulsa aleatoriamente la pelota hacia arriba
    public void Impulsa(float impulso)
    {
        rb.AddForce(GetNewDirection() * impulso, ForceMode2D.Impulse);
        impulsa = false;
    }

    //impulsa la pelota en una dirección concreta
    public void Impulsa(float impulso, Vector2 direccion)
    {
        rb.AddForce(direccion * impulso, ForceMode2D.Impulse);
        impulsa = false;
    }

    private Vector2 GetNewDirection()
    {
        Vector2 direction;

        int randY = 1;
        float randX = Random.Range(-0.5f, 0.5f); //valores entre -45º y 45º

        direction = new Vector2(randX, randY);

        return direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero;
        Vector3 hitPositionI = Vector3.zero;    //por si golpea muy cerca de otro tile a la izquierda
        Vector3 hitPositionD = Vector3.zero;    //igual pero derecha

        foreach (ContactPoint2D hit in collision.contacts)
        {
            hitPosition.x = hit.point.x;
            hitPosition.y = hit.point.y;

            hitPositionI.x = hit.point.x -0.1f;
            hitPositionI.y = hit.point.y;

            hitPositionD.x = hit.point.x + 0.1f;
            hitPositionD.y = hit.point.y;

            tileMap.SetTile(tileMap.WorldToCell(hitPosition), null);
            tileMap.SetTile(tileMap.WorldToCell(hitPositionI), null);
            tileMap.SetTile(tileMap.WorldToCell(hitPositionD), null);

            //Debug.Log(hitPosition.ToString() + " - " + hitPositionI.ToString() + " -> " + tileMap.WorldToCell(hitPosition) + " - " + tileMap.WorldToCell(hitPositionI));
        }

    }
}
