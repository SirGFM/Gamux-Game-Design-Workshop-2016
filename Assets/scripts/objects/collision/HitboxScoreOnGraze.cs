using UnityEngine;
using System.Collections;

public class HitboxScoreOnGraze : Hitbox {

	/** How much score the player gets per second
	 * when grazing something */
	public float scoreOnGraze = 6.0f;

	protected override void onGraze (Hitbox other) {
		Global.score.increase(this.scoreOnGraze * Time.fixedDeltaTime);
	}
}
