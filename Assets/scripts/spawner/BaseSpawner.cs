using UnityEngine;
using System.Collections;

public abstract class BaseSpawner : MonoBehaviour {

	/** Recycler that spawns objects */
	public Recycler recycler;

	/** Minimum time between spawns */
	public float minSpawnTime = 2.0f;

	/** Variance on the spawn time */
	public float randomTime = 1.0f;

	/** How long to wait until the next spawn */
	private float _cooldown = 0.0f;

	void Start() {
		this._cooldown = this.minSpawnTime;
	}

	abstract protected void spawn(GameObject go);

	void Update() {
		this._cooldown -= Time.deltaTime;
		if (this._cooldown <= 0.0f) {
			GameObject go;

			go = this.recycler.recycle();
			this.spawn(go);

			this._cooldown += this.minSpawnTime + Random.Range(0.0f, this.randomTime);
		}
	}
}
