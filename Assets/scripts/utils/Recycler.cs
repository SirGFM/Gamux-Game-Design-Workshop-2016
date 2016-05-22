using UnityEngine;
using System.Collections.Generic;

public class Recycler : MonoBehaviour {

	private LinkedList<GameObject> _recycled;

	public GameObject prefab;

	void Start () {
		this._recycled = new LinkedList<GameObject>();
	}

	/* Make sure all references are cleaned when this object is destroyed */
	void OnDestroy() {
		this._recycled.Clear();

		this._recycled = null;
	}

	/**
	 * Recycle a gameObject
	 *
	 * @return The gameObject
	 */
	public GameObject recycle() {
		GameObject go;

		if (this._recycled.Count == 0) {
			RecycleHook hook;

			/* Spawn a new gameObject, with the hook for being recycled later */
			go = GameObject.Instantiate<GameObject>(this.prefab);

			hook = go.GetComponent<RecycleHook>();
			if (hook == null) {
				/* Only adds the hook if it weren't present on the game object */
				hook = go.AddComponent<RecycleHook>();
			}

			hook.init(this);
		}
		else {
			go = this._recycled.First.Value;
			this._recycled.RemoveFirst();

			if (go != null) {
				go.SetActive(true);
			}
		}

		return go;
	}

	/**
	 * Add the gameObject back to the recycled list
	 *
	 * @param  [ in]go The game object
	 */
	public void free(GameObject go) {
		this._recycled.AddFirst(go);
	}
}
