using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	/** Shooting rate */
	public int bulletsPerSecond = 10;

	/** How long until we can shoot again */
	public float _cooldown = 0.0f;

	/** Component used to shooting */
	private BaseShooter _bullet;

	void OnDestroy() {
		this._bullet = null;
	}

	// Use this for initialization
	void Start () {
		this._bullet = this.GetComponent<BaseShooter>();
	}

	// Update is called once per frame
	void Update () {
		if (this._cooldown > 0) {
			this._cooldown -= Time.deltaTime;
		}

		if (this._bullet && Input.GetButton("Fire1") && this._cooldown <= 0) {
			this._bullet.shoot(90.0f);
			this._cooldown += 1.0f / (float)this.bulletsPerSecond;
		}
	}
}
