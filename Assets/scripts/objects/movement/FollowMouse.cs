using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : BaseMovement {

	/** How many pixels equals one unit */
	protected const float pixelsPerUnit = 32.0f;

	/** Position of the center of the game view, used to normalize
	 * the mouse position into the ranges [-w/2, w/2], [-h/2, h/2] */
	private Vector2 _offset;
	/** Factor that normalizes each axis into camera space */
	private Vector2 _viewNormalizer;

	void Start() {
		RawImage image;
		RectTransform gameView;

		gameView = GameObject.Find("Game View").GetComponent<RectTransform>();
		image = gameView.GetComponentInChildren<RawImage>();

		/* Simply set the offset as the image's center */
		this._offset = gameView.anchoredPosition;

		/* Convert the position to a normalized space (i.e.,
		 * transpose it to [-1, 1], [-1, 1]) */
		this._viewNormalizer.x = 2.0f / image.texture.width;
		this._viewNormalizer.y = 2.0f / image.texture.height;
		/* Convert it to camera space (i.e., transpose it to
		 * [-cam.w/2, cam.w/2], [-cam.h/2, cam.h/2]) */
		this._viewNormalizer.x *= (image.texture.width / pixelsPerUnit) * 0.5f;
		this._viewNormalizer.y *= (image.texture.height / pixelsPerUnit) * 0.5f;
		/* Scale it to the current pixel size */
		this._viewNormalizer /= gameView.rect.width / image.texture.width;
	}

	protected override void fixedUpdate () {
		Vector2 targetDirection;

		/* Calculate the mouse position within the game view */
		targetDirection = new Vector2(Input.mousePosition.x,
				Input.mousePosition.y);
		targetDirection -= this._offset;
		/* Convert it to camera space */
		targetDirection.x *= this._viewNormalizer.x;
		targetDirection.y *= this._viewNormalizer.y;

		/* Calculate the direction of the mouse relative to
		 * the game object */
		targetDirection -= new Vector2(this.transform.position.x,
				this.transform.position.y);
		/* Clamp to 0, if the mouse is less than 1 pixel away */
		if (Mathf.Abs(targetDirection.x) < (1.0f / pixelsPerUnit)) {
			targetDirection.x = 0.0f;
		}
		if (Mathf.Abs(targetDirection.y) < (1.0f / pixelsPerUnit)) {
			targetDirection.y = 0.0f;
		}

		if (targetDirection != Vector2.zero) {
			targetDirection.Normalize();
		}

		/* Set the current velocity */
		this.velocity = targetDirection * this.speed * Time.fixedDeltaTime;
	}
}
