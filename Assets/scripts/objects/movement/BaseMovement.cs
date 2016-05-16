using UnityEngine;

public abstract class BaseMovement : MonoBehaviour {

	/** Velocity at which the game object moves. Remember that 1
	 * unit equals 32 pixels */
	public float speed = 1.0f;

	/** The object's current velocity */
	[HideInInspector]
	public Vector2 velocity = Vector2.zero;

	/** Cached position after movement (updated on Update to
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
		translation = new Vector3(velocity.x, velocity.y);
		translation = translation.normalized * this.speed;
		translation *= Time.fixedDeltaTime;
		this.transform.Translate(translation);
	}

	void Update() {
		this.position = this.transform.position;
	}
}
