using UnityEngine;
using System.Collections;

public class Warp : MonoBehaviour {

	/** Clone of this game object */
	private GameObject _dummyClone;

	/** Hitbox of the clone */
	private BoxCollider2D _extraHitbox;

	/** Offset of the hitbox, if any */
	private Vector2 _hitboxOffset;

	/** Half the sprite's width */
	private float _width;

	/** Half the sprite's height */
	private float _height;

	void Start () {
		BoxCollider2D hitbox;
		SpriteRenderer originalSpr, cloneSpr;
		string name;

		originalSpr = this.GetComponent<SpriteRenderer>();
		hitbox = this.GetComponent<BoxCollider2D>();

		/* Retrieve the sprite's dimensions */
		this._width = ((float)originalSpr.sprite.texture.width) /
				originalSpr.sprite.pixelsPerUnit * 0.5f;
		this._height = ((float)originalSpr.sprite.texture.height) /
				originalSpr.sprite.pixelsPerUnit * 0.5f;

		/* Create a new object, out of view */
		name = this.gameObject.name + " rendering clone";
		this._dummyClone = new GameObject(name);
		this._dummyClone.transform.position = new Vector3(0.0f, 0.0f, -10.0f);

		/* Clone the object's sprite */
		cloneSpr = this._dummyClone.AddComponent<SpriteRenderer>();
		cloneSpr.sprite = originalSpr.sprite;
		cloneSpr.color = originalSpr.color;
		cloneSpr.material = originalSpr.material;
		cloneSpr.sortingLayerID = originalSpr.sortingLayerID;
		cloneSpr.sortingOrder = originalSpr.sortingOrder;

		/* Add a new hitbox, to be placed on the clone's position */
		this._extraHitbox = this.gameObject.AddComponent<BoxCollider2D>();
		this._extraHitbox.offset = hitbox.offset;
		this._extraHitbox.size = hitbox.size;
		/* Disable the hitbox (since it's only used when the clone if visible) */
		this._extraHitbox.enabled = false;
		/* Store the offset of the hitbox, so the cloned on may be correctly placed */
		this._hitboxOffset = hitbox.offset;
	}

	void FixedUpdate() {
		/** Dummy's new position */
		Vector3 dummyClonePos;
		/** Game object's cached position */
		Vector3 pos;

		/* Cache the position (beware!, it's a copy, not a reference!) */
		pos = this.transform.position;

		/* Warp the sprite horizontally */
		if (pos.x > Global.width) {
			this.transform.Translate(-Global.width * 2.0f, 0.0f, 0.0f);
		}
		else if (pos.x < -Global.width) {
			this.transform.Translate(Global.width * 2.0f, 0.0f, 0.0f);
		}
		
		/* Warp the sprite vertically */
		if (pos.y > Global.height) {
			this.transform.Translate(0.0f, -Global.height * 2.0f, 0.0f);
		}
		else if (pos.y < -Global.height) {
			this.transform.Translate(0.0f, Global.height * 2.0f, 0.0f);
		}

		/* Cache the position, again */
		pos = this.transform.position;

		/* Initially, set the dymmy out of view */
		dummyClonePos = new Vector3(pos.x, pos.y, -10.0f);

		/* NOTE: Limitation imposed by the algorithm
		 *
		 * There can be only one 'mirrored sprite' on screen, therefore
		 * the corners will seem broken, since the game object will
		 * first warp to right and upward, and then to the other direction.
		 *
		 * This could be solved by creating 3 clones and positioning the
		 * other 2 on these corner cases.
		 */

		/* Place the dummy mirrored on the horizontal axis */
		if (pos.x + this._width > Global.width) {
			dummyClonePos.x = pos.x - Global.width * 2.0f;
			dummyClonePos.z = pos.z;
		}
		else if (pos.x - this._width < -Global.width) {
			dummyClonePos.x = pos.x + Global.width * 2.0f;
			dummyClonePos.z = pos.z;
		}

		/* Place the dummy mirrored on the vertical axis */
		if (pos.y + this._height > Global.height) {
			dummyClonePos.y = pos.y - Global.height * 2.0f;
			dummyClonePos.z = pos.z;
		}
		else if (pos.y - this._height < -Global.height) {
			dummyClonePos.y = pos.y + Global.height * 2.0f;
			dummyClonePos.z = pos.z;
		}

		if (dummyClonePos.z == -10.0f) {
			/* Hide the hitbox and disable the dummy */
			this._extraHitbox.enabled = false;
			this._dummyClone.SetActive(false);
		}
		else {
			Vector2 offset;

			/* Enable and position the hitbox */
			this._extraHitbox.enabled = true;
			offset.x = dummyClonePos.x - pos.x;
			offset.y = dummyClonePos.y - pos.y;
			this._extraHitbox.offset = offset + this._hitboxOffset;
			/* Enable the dummy and set its position */
			this._dummyClone.SetActive(true);
			this._dummyClone.transform.position = dummyClonePos;
		}
	}
}
