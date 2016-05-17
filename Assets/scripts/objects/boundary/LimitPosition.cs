using Mathf = UnityEngine.Mathf;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using Vector3 = UnityEngine.Vector3;

public class LimitPosition : MonoBehaviour {

	private void doUpdate() {
		Vector3 delta;

		/* Do nothing if the object is within the valid area */
		if (Mathf.Abs(this.transform.position.x) <= Global.width &&
		    	Mathf.Abs(this.transform.position.y) <= Global.height) {
			return;
		}

		delta = Vector3.zero;
		/* Limit the object horizontally */
		if (this.transform.position.x > Global.width) {
			delta.x = Global.width - this.transform.position.x;
		}
		else if (this.transform.position.x < -Global.width) {
			delta.x = -Global.width - this.transform.position.x;
		}
		/* Limit the object vertically */
		if (this.transform.position.y > Global.height) {
			delta.y = Global.height - this.transform.position.y;
		}
		else if (this.transform.position.y < -Global.height) {
			delta.y = -Global.height - this.transform.position.y;
		}

		this.transform.Translate(delta);
	}

	void FixedUpdate () { this.doUpdate(); }
	void Update () { this.doUpdate(); }
}
