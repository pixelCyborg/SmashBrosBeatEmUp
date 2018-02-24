using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour {
    public List<CrossbowUpgrade> upgrades = new List<CrossbowUpgrade>();
    public GameObject boltPrefab;
    public static Crossbow instance;

    private static bool CanShoot = true;
    public int damage = 1;
    public float reloadTime = 0.5f;
    public float boltSpeed = 1.0f;
    public List<Property> properties;

    private void Start()
    {
        instance = this;
        properties = new List<Property>();
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
        if (CanShoot && !Inventory.open && !TravelMap.mapShown)
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

            CameraShake.AddShake(damage * 0.1f);
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
        CrossbowBolt bolt = Instantiate(instance.boltPrefab, instance.transform.position, Quaternion.identity).GetComponent<CrossbowBolt>();
        bolt.properties = properties;
        bolt.Shoot(x, y, damage);
    }
}
