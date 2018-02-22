using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : Skeleton {
	private bool CanShoot = false;
	public GameObject boltPrefab;
	public float reloadTime;
	public float boltSpeed;


	internal override void Attack (Transform target)
	{
		Vector2 direction = (target.position - transform.position).normalized;
		direction *= boltSpeed;
		Shoot (direction.x, direction.y);
	}

	void Reload()
	{
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
		CrossbowBolt bolt = Instantiate(boltPrefab, transform.position, Quaternion.identity).GetComponent<CrossbowBolt>();
		bolt.Shoot(x, y, damage);
	}
}
