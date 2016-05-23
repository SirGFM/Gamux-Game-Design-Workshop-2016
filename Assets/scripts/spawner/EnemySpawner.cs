using UnityEngine;
using System.Collections;

public class EnemySpawner : BaseSpawner {

	/** Bullet recycler to set on the GO, since
	 * it can't be done from the prefab */
	public Recycler bulletRecycler;

	protected override void spawn (GameObject go) {
		BaseShooter shooter;

		shooter = go.GetComponent<BaseShooter>();
		if (shooter != null) {
			shooter.recycler = this.bulletRecycler;
		}
	}
}
