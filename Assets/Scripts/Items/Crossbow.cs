using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour {
    Transform origin;
    public List<CrossbowUpgrade> upgrades = new List<CrossbowUpgrade>();
    public GameObject boltPrefab;
    public static Crossbow instance;

    private static bool CanShoot = true;
    public int damage = 1;
    public float reloadTime = 0.5f;
    public float boltSpeed = 1.0f;
    public List<Property> properties;
    public WeaponAudioHandler audioHandler;

    private void Start()
    {
        origin = GetComponentInParent<Player>().transform;
        instance = this;
        properties = new List<Property>();
        audioHandler.Initialize(GetComponent<AudioSource>());
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
            audioHandler.PlayShoot();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - transform.position);
            direction = direction.normalized * boltSpeed;
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
        CrossbowBolt bolt = Instantiate(instance.boltPrefab, instance.transform.position, Quaternion.identity).GetComponent<CrossbowBolt>();
        bolt.properties = properties;
        bolt.Shoot(x, y, damage);
    }
}
