using UnityEngine;
using System.Collections;

public class FollowTarget : BaseMovement {

	/** GameObject that is being targeted */
	public BaseMovement target;

	protected override void fixedUpdate () {
		/* Set the velocity so the GO follows were its
		 * target is gonna be on the next turn */
		this.velocity = (this.target.position + this.target.velocity * Time.deltaTime) - this.position;
	}

}
