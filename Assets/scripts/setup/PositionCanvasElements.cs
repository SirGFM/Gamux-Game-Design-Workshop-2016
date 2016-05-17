using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PositionCanvasElements : MonoBehaviour {

	/** Initialize the game view in a resolution independent way */
	void Start () {
		/** Game object that represents the game view (i.e., canvas object
	 	 * un which the game camera will be displayed) */
		RectTransform gameView;
		/** The panel at the right side of the screen, which may be shrunk to
	 	 * free up space to the game view, if necessary */
		RectTransform panel;
		/** The input image */
		RawImage image;
		/** Central position of the texture within the canvas */
		Vector2 textureCenter;
		/** Factor by which each pixel is scaled */
		float zoom;
		float zoomx, zoomy;
		/** Half the dimension of the texture */
		int imageHalfWidth;

		gameView = GameObject.Find("Game View").GetComponent<RectTransform>();
		panel = GameObject.Find("Game Status").GetComponent<RectTransform>();

		/* Calculate how much is half the texture's width,
		 * to check if it would fit at the canvas' left half */
		image = gameView.GetComponentInChildren<RawImage>();
		imageHalfWidth = (int)Mathf.Floor(image.texture.width * 0.5f);
		/* Calculate the texture position so it stays at the
		 * center of the canvas' left half */
		textureCenter = new Vector2(Camera.main.pixelWidth * 0.25f,
				Camera.main.pixelHeight * 0.5f);

		/* Calculate the zoom factor */
		zoomx = (Camera.main.pixelWidth * 0.5f) / image.texture.width;
		zoomy = Camera.main.pixelHeight / image.texture.height;
		zoom = Mathf.Floor(Mathf.Min(zoomx, zoomy));

		/* If the image wouldn't fit on the left half,
		 * push the status panel to the right */
		if (textureCenter.x < imageHalfWidth) {
			Vector2 pos;

			textureCenter.x = imageHalfWidth;

			pos = new Vector2(image.texture.width * zoom / Camera.main.pixelWidth,
					panel.anchorMin.y);
			panel.anchorMin = pos;
		}

		/* Set the image position */
		gameView.anchorMin = Vector2.zero;
		gameView.anchorMax = Vector2.zero;
		gameView.anchoredPosition = textureCenter;

		/* Set the image dimensions */
		gameView.sizeDelta = zoom * (new Vector2(image.texture.width,
				image.texture.height));

		/* Update the cached values */
		Global.updateParameters();
	}
}
