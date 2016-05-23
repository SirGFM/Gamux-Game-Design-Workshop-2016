using UnityEngine;
using System.Collections;

public class BaseController : MonoBehaviour {

	/** Shooting rate */
	public float bulletsPerSecond = 10.0f;

	/** How long until we can shoot again */
	private float _cooldown = 0.0f;

	/** Component used to shooting */
	protected BaseShooter _shooter;

	/** This GO's BaseMovement */
	protected BaseMovement move;

	void OnDestroy() {
		this._shooter = null;
	}

	// Use this for initialization
	void Start () {
		this._shooter = this.GetComponent<BaseShooter>();
		this.move = this.GetComponent<BaseMovement>();
	}

	/**
	 * Function to be overriden on child classes to enable shooting
	 *
	 * @return Whether the GO is shooting on this frame
	 */
	virtual protected bool isShooting() {
		return false;
	}

	/**
	 * Override to change the angle the GO should shoot at
	 *
	 * @return The shooting angle (defaults to upward)
	 */
	virtual protected float getShootingAngle() {
		return 90.0f;
	}

	// Update is called once per frame
	void Update () {
		if (this._cooldown > 0) {
			this._cooldown -= Time.deltaTime;
		}

		if (this._shooter && this.isShooting() && this._cooldown <= 0) {
			this._shooter.shoot(this.getShootingAngle());
			this._cooldown += 1.0f / (float)this.bulletsPerSecond;
		}
	}
}
