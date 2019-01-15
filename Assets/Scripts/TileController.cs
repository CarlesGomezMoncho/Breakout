﻿using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{

    private Tilemap tileMap;

    void Start()
    {
        tileMap = GetComponent<Tilemap>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(tileMap.GetTile(Vector3Int.RoundToInt(0,0,0)));
        //Debug.Log(tileMap.GetTile(Vector3Int.RoundToInt(collision.transform.position)));
        Vector3Int tilePos = tileMap.WorldToCell(collision.transform.position);

        tileMap.SetTile(tilePos, null);
    }
}
