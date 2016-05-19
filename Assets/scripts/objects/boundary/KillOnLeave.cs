using UnityEngine;
using System.Collections;

public class KillOnLeave : MonoBehaviour {

	void Update () {
		Vector3 pos;

		pos = this.transform.position;

		if ((Mathf.Abs(pos.x) > Global.width + 1.0f) ||
				(Mathf.Abs(pos.y) > Global.height + 1.0f)) {
			this.gameObject.SetActive(false);
		}
	}
}
