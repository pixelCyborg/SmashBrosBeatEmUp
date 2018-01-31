using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour {
    public List<CrossbowUpgrade> upgrades = new List<CrossbowUpgrade>();
    public GameObject boltPrefab;
    private static Crossbow instance;

    private static bool CanShoot = true;
    public int damage = 1;
    public float reloadTime = 0.5f;
    public float boltSpeed = 1.0f;

    private void Start()
    {
        instance = this;
    }

    void GetUpgrades()
    {
        upgrades = new List<CrossbowUpgrade>();
        CrossbowUpgrade[] upgradeArray = transform.GetComponents<CrossbowUpgrade>();
        foreach (CrossbowUpgrade upgrade in upgradeArray)
        {
            upgrades.Add(upgrade);
        }
    }

    public void FireCrossbow()
    {
        if (CanShoot)
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
            direction *= boltSpeed;

            Reload();
            Shoot(direction.x, direction.y);

            GetUpgrades();
            foreach (CrossbowUpgrade upgrade in upgrades)
            {
                upgrade.OnShoot(direction.x, direction.y);
            }
        }
    }

    void Reload()
    {
        foreach(CrossbowUpgrade upgrade in upgrades)
        {
            upgrade.OnReload();
        }
        StartCoroutine(_Reload());
    }

    IEnumerator _Reload()
    {
        CanShoot = false;
        yield return new WaitForSeconds(reloadTime);
        CanShoot = true;
    }

    public void Shoot(float x, float y)
    {
        GameObject bolt = Instantiate(instance.boltPrefab, instance.transform.position, Quaternion.identity);
        bolt.GetComponent<CrossbowBolt>().Shoot(x, y);
    }
}
