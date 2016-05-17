using UnityEngine;

public abstract class BaseMovement : MonoBehaviour {

	/** Velocity at which the game object moves. Remember that 1
	 * unit equals 32 pixels */
	public float speed = 1.0f;

	/** The object's current velocity */
	//[HideInInspector]
	public Vector2 velocity {
		/** Set the instantaneous velocity */
		protected set {
			this._curVelocity = value;
		}
		/** Retrieve the velocity on the previous frame (so it's
		 * uniformly returned to all objects) */
		get {
			return this._cachedVelocity;
		}
	}
	/** The velocity at which the object is moving */
	private Vector2 _curVelocity = Vector2.zero;
	/** Cached velocity, updated on Update() to keep the value
	 * uniform independent of whoever is reading the value */
	private Vector2 _cachedVelocity = Vector2.zero;

	/** Cached position after movement (updated on Update() to
	 * keep it uniform through all objects) */
	[HideInInspector]
	public Vector2 position = Vector2.zero;

	/** Overload-able function where movement should be implemented
	 * by setting the velocity */
	abstract protected void fixedUpdate();

	void FixedUpdate () {
		Vector3 translation;

		/** Call the user defined movement */
		this.fixedUpdate();

		/** Integrate the position (using Euler) */
		translation = new Vector3(this._curVelocity.x, this._curVelocity.y);
		translation = translation.normalized * this.speed;
		translation *= Time.fixedDeltaTime;
		this.transform.Translate(translation);
	}

	void Update() {
		this.position = this.transform.position;
		this._cachedVelocity = this._curVelocity;
	}
}
