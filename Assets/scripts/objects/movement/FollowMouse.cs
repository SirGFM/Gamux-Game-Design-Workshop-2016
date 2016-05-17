using Input = UnityEngine.Input;
using Mathf = UnityEngine.Mathf;
using Vector2 = UnityEngine.Vector2;

public class FollowMouse : BaseMovement {

	protected override void fixedUpdate () {
		Vector2 targetDirection;

		/* Retrieve the mouse position within the game space */
		targetDirection = new Vector2(Input.mousePosition.x,
				Input.mousePosition.y);
		targetDirection = Global.windowSpaceToGameSpace(targetDirection);

		/* Calculate the direction of the mouse relative to
		 * the game object */
		targetDirection -= new Vector2(this.transform.position.x,
				this.transform.position.y);
		/* Clamp to 0, if the mouse is less than 1 pixel away */
		if (Mathf.Abs(targetDirection.x) < (1.0f / Global.pixelsPerUnit)) {
			targetDirection.x = 0.0f;
		}
		if (Mathf.Abs(targetDirection.y) < (1.0f / Global.pixelsPerUnit)) {
			targetDirection.y = 0.0f;
		}

		/* Set the current velocity */
		this.velocity = targetDirection;
	}
}
