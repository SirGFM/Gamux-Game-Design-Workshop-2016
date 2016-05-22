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

	/** Initial hitpoints */
	public float maxHealth = 1.0f;

	/** Current health */
	private float _health;

	/** Amount of damage delt on collision */
	public float damage = 1.0f;

	void OnEnable() {
		this._health = this.maxHealth;
	}

	void Start() {
		Rigidbody2D rb;
		
		rb = this.GetComponent<Rigidbody2D>();
		rb.isKinematic = true;
		rb.gravityScale = 0.0f;
	}

	/**
	 * Called by the default onHit implementation. By
	 * default, it simply deactivates this object.
	 */
	virtual protected void onDeath() {
		this.gameObject.SetActive(false);
	}

	/**
	 * Called whenever the object is hit by something
	 *
	 * The default implementation decreases this objects'
	 * health by the other's damage and calls onDeath if it
	 * goes bellow 0 (actually, if it's less or equal to).
	 *
	 * @param  [ in]other The offending hitbox
	 */
	virtual protected void onHit(Hitbox other) {
		this._health -= other.damage;
		if (this._health <= 0) {
			this.onDeath();
		}
	}

	/**
	 * Called whenever the object touches another one but
	 * evades its hitbox. The default implementation does
	 * nothing.
	 */
	virtual protected void onGraze(Hitbox other) {}

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

		/* Whatever happens, execute only the current object's
		 * callback, since it will be run once again for the
		 * other one */
		if (sqrDist <= sqrRadius) {
			this.onHit(otherHb);
		}
		else {
			this.onGraze(otherHb);
		}
	}

	/* Draw the actual hitbox on the editor */
	void OnDrawGizmos() {
		Color original;

		original = UnityEditor.Handles.color;
		UnityEditor.Handles.color = this.color;

		UnityEditor.Handles.DrawWireDisc(this.transform.position + this.offset,
				Vector3.back, this.radius);

		UnityEditor.Handles.color = original;
	}
}
