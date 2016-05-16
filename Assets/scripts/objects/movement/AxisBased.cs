using UnityEngine;

public class AxisBased : BaseMovement {
	protected override void fixedUpdate () {
		this.velocity.x = Input.GetAxis("Horizontal");
		this.velocity.y = Input.GetAxis("Vertical");
	}
}
