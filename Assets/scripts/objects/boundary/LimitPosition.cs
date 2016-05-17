using UnityEngine;
using UnityEngine.UI;

public class LimitPosition : MonoBehaviour {

	/** How many pixels equals one unit */
	protected const float pixelsPerUnit = 32.0f;

	/** Maximum position, after which the game object will be blocked */
	private Vector2 _maxPosition;

	void Start () {
		RawImage image;
		RectTransform gameView;

		gameView = GameObject.Find("Game View").GetComponent<RectTransform>();
		image = gameView.GetComponentInChildren<RawImage>();

		/* Convert it to camera space (i.e., transpose it to
		 * [-cam.w/2, cam.w/2], [-cam.h/2, cam.h/2]) */
		this._maxPosition.x = image.texture.width / pixelsPerUnit;
		this._maxPosition.y = image.texture.height / pixelsPerUnit;
		this._maxPosition *= 0.5f;
	}

	private void doUpdate() {
		Vector3 delta;

		if (Mathf.Abs(this.transform.position.x) <= this._maxPosition.x &&
		    	Mathf.Abs(this.transform.position.y) <= this._maxPosition.y) {
			return;
		}

		delta = Vector3.zero;
		if (this.transform.position.x > this._maxPosition.x) {
			delta.x = this._maxPosition.x - this.transform.position.x;
		}
		else if (this.transform.position.x < -this._maxPosition.x) {
			delta.x = -this._maxPosition.x - this.transform.position.x;
		}
		if (this.transform.position.y > this._maxPosition.y) {
			delta.y = this._maxPosition.y - this.transform.position.y;
		}
		else if (this.transform.position.y < -this._maxPosition.y) {
			delta.y = -this._maxPosition.y - this.transform.position.y;
		}

		this.transform.Translate(delta);
	}

	void FixedUpdate () { this.doUpdate(); }
	void Update () { this.doUpdate(); }
}
