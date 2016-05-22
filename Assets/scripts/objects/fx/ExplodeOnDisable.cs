using UnityEngine;
using System.Collections;

public class ExplodeOnDisable : MonoBehaviour {

	void OnDisable() {
		Vector3 pos;

		pos = this.transform.position;
		if (Global.explosionRecycler != null &&
				Mathf.Abs(pos.x) < Global.width &&
				Mathf.Abs(pos.y) < Global.height) {
			GameObject go;

			go = Global.explosionRecycler.recycle();
			if (go != null) {
				go.transform.position = pos;
			}
		}
	}
}
