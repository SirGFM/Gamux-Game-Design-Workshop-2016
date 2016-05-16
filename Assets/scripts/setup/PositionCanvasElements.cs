using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PositionCanvasElements : MonoBehaviour {

	/** Game object that represents the game view (i.e., canvas object
	 * un which the game camera will be displayed) */
	public RectTransform gameView;

	/** The panel at the right side of the screen, which may be shrunk to
	 * free up space to the game view, if necessary */
	public RectTransform panel;

	/** Initialize the game view in a resolution independent way */
	void Start () {
		/** The input image */
		RawImage image;
		/** Central position of the texture within the canvas */
		Vector2 textureCenter;
		/** Half the dimension of the texture */
		int imageHalfWidth;

		/* Calculate how much is half the texture's width,
		 * to check if it would fit at the canvas' left half */
		image = this.gameView.GetComponent<RawImage>();
		imageHalfWidth = (int)Mathf.Floor(image.texture.width * 0.5f);
		/* Calculate the texture position so it stays at the
		 * center of the canvas' left half */
		textureCenter = new Vector2(Camera.main.pixelWidth * 0.25f,
				Camera.main.pixelHeight * 0.5f);

		/* If the image wouldn't fit on the left half,
		 * push the status panel to the right */
		if (textureCenter.x < imageHalfWidth) {
			Vector2 pos;

			textureCenter.x = imageHalfWidth;

			pos = new Vector2(image.texture.width / Camera.main.pixelWidth,
					panel.anchorMin.y);
			panel.anchorMin = pos;
		}

		/* Set the image position */
		this.gameView.anchorMin = Vector2.zero;
		this.gameView.anchorMax = Vector2.zero;
		this.gameView.anchoredPosition = textureCenter;

		/* Calculate the zoom factor */
		/* Set the image dimensions */
		this.gameView.sizeDelta = new Vector2(image.texture.width,
				image.texture.height);
	}
}
