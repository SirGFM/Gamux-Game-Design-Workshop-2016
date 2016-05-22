using UnityEngine;
using System.Collections;

public class DisableOnAnimFinished : MonoBehaviour {

	public void disable() {
		this.gameObject.SetActive(false);
	}

}
