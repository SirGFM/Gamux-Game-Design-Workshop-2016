using UnityEngine;
using System.Collections;

public class BasicShooter : BaseShooter {

	public override void shoot (float angle) {
		float deltaAngle, curAngle;
		int i;

		/* Calculate the initial angle and its variation between bullets */
		if (this.count > 1) {
			deltaAngle = this.angleRange / (float)(this.count - 1);
			curAngle = angle - this.angleRange * 0.5f;
		}
		else {
			deltaAngle = 0.0f;
			curAngle = angle;
		}

		i = 0;
		while (i < this.count) {
			GameObject go;
			ConstantMovement constMove;

			go = this.recycler.recycle();
			go.transform.position = this.offset + this.transform.position;

			constMove = go.GetComponent<ConstantMovement>();

			if (constMove != null) {
				constMove.angle = curAngle;
			}

			i++;
			curAngle += deltaAngle;
		}
	}
}
