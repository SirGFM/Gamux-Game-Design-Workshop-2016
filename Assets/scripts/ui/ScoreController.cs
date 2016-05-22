using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

	/** Format used to display the score */
	private const string format = "0000000";

	/** Text used to display the score */
	private Text _scoreText;

	/** Current score */
	private float _scoreVal = 0.0f;

	/** Score's value that is slowly incremented */
	private float _displayScore = 0.0f;

	private float _delta = 0.0f;

	void Start () {
		this._scoreText = GameObject.Find("Score Text").GetComponent<Text>();
		this._scoreVal = 0.0f;
		this._displayScore = 0.0f;
		this._delta = 0.0f;
		this._scoreText.text = this._displayScore.ToString(ScoreController.format);
	}

	void Update () {
		if (this._displayScore < this._scoreVal) {
			this._displayScore += this._delta * Time.deltaTime;

			if (this._displayScore >= this._scoreVal) {
				this._displayScore = this._scoreVal;
				this._delta = 0.0f;
			}

			this._scoreText.text = this._displayScore.ToString(ScoreController.format);
		}
	}

	/**
	 * Increase the score by an amount
	 *
	 * @param  [ in]value Added score
	 */
	public void increase(float value) {
		this._scoreVal += value;
		this._delta += value;
	}
}
