using Input = UnityEngine.Input;
using Vector2 = UnityEngine.Vector2;

public class AxisBased : BaseMovement {

	protected override void fixedUpdate () {
		this.velocity = new Vector2(Input.GetAxisRaw("Horizontal"),
				Input.GetAxisRaw("Vertical"));
	}
}
