using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour {
    public GameObject boltPrefab;
    private static Crossbow instance;

    private static bool CanShoot = true;
    public float reloadTime = 0.5f;
    public float boltSpeed = 1.0f;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (CanShoot && Input.GetButtonDown("Fire1"))
        {

            /*
            Vector3 direction = transform.localScale;
            direction.y = 0;
            if(Input.GetAxis("Vertical") < -0.2f)
            {
                direction.y = -1;
                direction.x = 0;
            }
            else if(Input.GetAxis("Vertical") > 0.2f)
            {
                direction.y = 1;
                direction.x = 0;
            }
            */
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - transform.position).normalized;
            Debug.Log(direction);
            direction *= boltSpeed;

            StartCoroutine(Reload());
            Crossbow.Shoot(direction.x, direction.y);
        }
    }

    IEnumerator Reload()
    {
        CanShoot = false;
        yield return new WaitForSeconds(reloadTime);
        CanShoot = true;
    }

    public static void Shoot(float x, float y)
    {
        GameObject bolt = Instantiate(instance.boltPrefab, instance.transform.position, Quaternion.identity);
        bolt.GetComponent<CrossbowBolt>().Shoot(x, y);
    }
}
