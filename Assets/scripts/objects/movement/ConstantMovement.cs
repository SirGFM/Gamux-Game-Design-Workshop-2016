using ExecuteInEditMode = UnityEngine.ExecuteInEditMode;
using Vector2 = UnityEngine.Vector2;
using Mathf = UnityEngine.Mathf;

public class ConstantMovement : BaseMovement {

	/** Angle of the movement, in degrees; 0 is to the right
	 * and 90 is upward */
	private float _angle;
	public float angle {
		set {
			float rad;

			this._angle = value;

			rad = this._angle * Mathf.Deg2Rad;
			this._direction.x = Mathf.Cos(rad);
			this._direction.y = Mathf.Sin(rad);
		}
		get {
			return this._angle;
		}
	}

	/** Direction of the movement */
	private Vector2 _direction = Vector2.down;

	protected override void fixedUpdate () {
		this.velocity = this._direction;
	}
}
