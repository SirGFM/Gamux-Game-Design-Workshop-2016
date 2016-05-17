using UnityEngine;

public class AxisBased : BaseMovement {

	protected override void fixedUpdate () {
		this.velocity = new Vector2(Input.GetAxis("Horizontal"),
				Input.GetAxis("Vertical"));
	}
}
