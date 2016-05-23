//using Math = System.Math;
using Vector2 = UnityEngine.Vector2;
using Mathf = UnityEngine.Mathf;

public class EnemyAimController : BaseController {

	protected override float getShootingAngle () {
		Vector2 from, to, tgt;

		from = this.move.position + new Vector2(this._shooter.offset.x, this._shooter.offset.y);
		to = Global.player.position + Global.player.velocity +
				new Vector2(Global.playerHitbox.offset.x, Global.playerHitbox.offset.y);
		tgt = to - from;

		return Mathf.Atan2(tgt.y, tgt.x) * Mathf.Rad2Deg;
	}

	protected override bool isShooting () {
		return true;
	}
}
