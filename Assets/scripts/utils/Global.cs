using UnityEngine;
using UnityEngine.UI;

public class Global {

	/** How many pixels equals one unit */
	public const float pixelsPerUnit = 32.0f;

	/** Half the width of the game space. Considers the game area is
	 * centered at (0,0) and it expands in both directions by width */
	static private float _width;

	/** Half the width of the game space. Considers the game area is
	 * centered at (0,0) and it expands in both directions by width (RO) */
	static public float width {
		get {
			return Global._width;
		}
	}

	/** Half the height of the game space. Considers the game area is
	 * centered at (0,0) and it expands in both directions by height */
	static private float _height;

	/** Half the height of the game space. Considers the game area is
	 * centered at (0,0) and it expands in both directions by height (RO) */
	static public float height {
		get {
			return Global._height;
		}
	}

	/** Position of the center of the game view, used to normalize
	 * the mouse position into the ranges [-w/2, w/2], [-h/2, h/2] */
	static private Vector2 _offset;

	/** Factor that normalizes each axis into game space */
	static private Vector2 _viewNormalizer;

	/** Recycler of asteroids */
	static private Recycler _asteroidRecycler;
	/** Recycler of asteroids (RO) */
	static public Recycler asteroidRecycler {
		get {
			return Global._asteroidRecycler;
		}
	}

	/** Recycler of explosions */
	static private Recycler _explosionRecycler;
	/** Recycler of explosions (RO) */
	static public Recycler explosionRecycler {
		get {
			return Global._explosionRecycler;
		}
	}

	/** Update all cached parameters */
	static public void updateParameters() {
		RawImage image;
		RectTransform gameView;

		gameView = GameObject.Find("Game View").GetComponent<RectTransform>();
		image = gameView.GetComponentInChildren<RawImage>();

		Global._width = image.texture.width / Global.pixelsPerUnit;
		Global._width *= 0.5f;
		Global._height = image.texture.height / Global.pixelsPerUnit;
		Global._height *= 0.5f;

		/* Simply set the offset as the image's center */
		Global._offset = gameView.anchoredPosition;

		/* Convert the position to a normalized space (i.e.,
		 * transpose it to [-1, 1], [-1, 1]) */
		Global._viewNormalizer.x = 2.0f / image.texture.width;
		Global._viewNormalizer.y = 2.0f / image.texture.height;
		/* Convert it to camera space (i.e., transpose it to
		 * [-cam.w/2, cam.w/2], [-cam.h/2, cam.h/2]) */
		Global._viewNormalizer.x *= Global._width;
		Global._viewNormalizer.y *= Global._height;
		/* Scale it to the current pixel size */
		Global._viewNormalizer /= gameView.rect.width / image.texture.width;

		/* Retrieve all recyclers */
		Global._asteroidRecycler = GameObject.Find("Asteroid Recycler").GetComponent<Recycler>();
		Global._explosionRecycler = GameObject.Find("Explosion Recycler").GetComponent<Recycler>();
	}

	/**
	 * Convert a point from window space to game space
	 *
	 * @param  [ in]point The point to be converted
	 * @return            The converted point
	 */
	static public Vector2 windowSpaceToGameSpace(Vector2 point) {
		Vector2 ret;

		ret = point - Global._offset;
		ret.x *= Global._viewNormalizer.x;
		ret.y *= Global._viewNormalizer.y;

		return ret;
	}
}
