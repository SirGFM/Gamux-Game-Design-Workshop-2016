using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using NonSerializedAttribute = System.NonSerializedAttribute;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Hitbox : MonoBehaviour {

	/** Color of the hitbox */
	public Color color = Color.white;

	/** Radius of the hitbox */
	public float radius;

	/** Offset of the hitbox from the gameObject's center */
	public Vector3 offset;

	/** Functions to be called on collision */
	public UnityEvent onCollide;

	/** Functions to be called when the objects only graze */
	public UnityEvent onGraze;

	void Start() {
		Rigidbody2D rb;
		
		rb = this.GetComponent<Rigidbody2D>();
		rb.isKinematic = true;
		rb.gravityScale = 0.0f;
	}

	void OnDestroy() {
		this.onCollide.RemoveAllListeners();
		this.onGraze.RemoveAllListeners();

		this.onCollide = null;
		this.onGraze = null;
	}

	void OnTriggerEnter2D(Collider2D other) {
		checkCollision(other);
	}
	void OnTriggerStay2D(Collider2D other) {
		checkCollision(other);
	}

	/**
	 * Check if a collision actually happened, instead of only grazing
	 */
	private void checkCollision(Collider2D other) {
		Hitbox otherHb;
		Vector3 posA, posB;
		float distX, distY, sqrDist, sqrRadius;

		try {
			otherHb = other.GetComponent<Hitbox>();
		}
		catch {
			return;
		}

		posA = this.transform.position;
		posB = other.transform.position;

		/* If the distance between both objects is greater than the
		 * length of the sum of the hiboxes, then the objects
		 * aren't touching */
		distX = (posA.x + offset.x) - (posB.x + otherHb.offset.x);
		distY = (posA.y + offset.y) - (posB.y + otherHb.offset.y);
		sqrDist = distX * distX + distY * distY;

		sqrRadius = this.radius + otherHb.radius;
		sqrRadius *= sqrRadius;

		if (sqrDist <= sqrRadius) {
			/* If they did collide, execute only the current object's
			 * callback, since it will be run once again for the
			 * other one */
			try {
				this.onCollide.Invoke();
				this.onGraze.Invoke();
			} catch {}
		}
	}

	/* Draw the actual hitbox on the editor */
	void OnDrawGizmos() {
		Color original;

		original = UnityEditor.Handles.color;
		UnityEditor.Handles.color = this.color;

		UnityEditor.Handles.DrawWireDisc(this.transform.position + this.offset,
				Vector3.back,
				this.radius);

		UnityEditor.Handles.color = original;
	}
}
