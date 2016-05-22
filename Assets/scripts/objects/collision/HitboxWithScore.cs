using UnityEngine;
using System.Collections;

public class HitboxWithScore : Hitbox {

	/** How much score the player gets after killing this GO */
	public float score;

	protected override void onDeath () {
		base.onDeath ();
		Global.score.increase(this.score);
	}
}
