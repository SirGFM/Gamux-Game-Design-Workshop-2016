﻿using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.R)) {
			Application.LoadLevel("game_scene");
		}
	}
}
