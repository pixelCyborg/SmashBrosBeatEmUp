using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bouncy : MonoBehaviour {
    public static Tilemap tilemap;
    public static Bouncy instance;
    public Tile bouncyTile;

    private void Start()
    {
        instance = this;
        tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
        if (body == null) return;
        body.AddForce(collision.relativeVelocity * -1, ForceMode2D.Impulse);
    }

    public void SetBouncy(Vector3 worldPos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(worldPos);
        Debug.Log(cellPos);
        tilemap.SetTile(cellPos, bouncyTile);
        StartCoroutine(_SetBouncy(cellPos));
    }

    IEnumerator _SetBouncy(Vector3Int cellPos)
    {
        yield return new WaitForSeconds(5.0f);
        tilemap.SetTile(cellPos, null);
    }
}
