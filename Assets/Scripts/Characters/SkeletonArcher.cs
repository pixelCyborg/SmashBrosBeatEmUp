using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : Skeleton {
	private bool canShoot = true;
	public GameObject boltPrefab;
	public float reloadTime;
	public float boltSpeed;

    //For prototype sprite;
    public GameObject hat;

    internal override void Attack(Transform target)
    {
        Vector2 direction = (target.position - transform.position).normalized;
        direction *= boltSpeed;

        if (canShoot)
        {
            Shoot(direction.x, direction.y);
            Reload();
        }
    }

 /*   internal override void PursueTarget(Transform target)
    {
        base.Move();
        //base.PursueTarget(target);
    } */

    void Reload()
	{
		StartCoroutine(_Reload());
	}

	IEnumerator _Reload()
	{
		canShoot = false;
		yield return new WaitForSeconds(reloadTime);
		canShoot = true;
	}

	public void Shoot(float x, float y)
	{
		SkeletonFireball bolt = Instantiate(boltPrefab, transform.position, Quaternion.identity).GetComponent<SkeletonFireball>();
		bolt.Shoot(x, y, damage);
	}

    internal override void OnDie()
    {
        base.OnDie();
        Destroy(hat);
    }
}
