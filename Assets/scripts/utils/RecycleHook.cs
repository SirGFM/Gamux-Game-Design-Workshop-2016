using UnityEngine;
using System.Collections;

/** Hook that forces the object to be recycled, on disable */
public class RecycleHook : MonoBehaviour {

	/** The object that keeps track of this object */
	private Recycler _parent;

	public void init(Recycler parent) {
		this._parent = parent;
	}

	void OnDisable() {
		/* Avoids error depending on the order the game is cleaned */
		try{
			this._parent.free (this.gameObject);
		} catch {}
	}

	void OnDestroy() {
		this._parent = null;
	}
}
