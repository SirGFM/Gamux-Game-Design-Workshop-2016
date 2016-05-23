#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

public class RandomSpawner : BaseSpawner {

	/** Color of the spawner */
	public Color spawnColor = Color.white;

	/** Area from which objects may be spawned */
	public Rect spawnArea;

	/** Minimum angle toward which the object will move */
	public float minAngle = 270.0f;

	/** Variance on the angle */
	public float randomAngle = 0.0f;

	/** Minimum speed at which objects move */
	public float minSpeed = 1.5f;

	/** Variance on the speed */
	public float randomSpeed = 0.5f;

	override protected void spawn (GameObject go) {
		SpriteRenderer spr;
		ConstantMovement move;
		Vector3 pos;

		pos.x = this.spawnArea.x + Random.Range(0.0f, this.spawnArea.width);
		pos.y = this.spawnArea.y + Random.Range(0.0f, this.spawnArea.height);
		pos.z = 0.0f;
		go.transform.position = pos;
		
		move = go.GetComponent<ConstantMovement>();
		if (move != null) {
			move.angle = this.minAngle + Random.Range(0.0f, this.randomAngle);
			move.speed = this.minSpeed + Random.Range(0.0f, this.randomSpeed);
		}
		spr = go.GetComponent<SpriteRenderer>();
		if (spr != null) {
			/* TODO Set the color */
		}
	}

#if UNITY_EDITOR
	/* Draw the actual hitbox on the editor */
	void OnDrawGizmos() {
		Vector3 []points;
		Color original;

		original = UnityEditor.Handles.color;
		UnityEditor.Handles.color = this.spawnColor;

		points = new Vector3[5];
		points[0].x = this.spawnArea.x;
		points[0].y = this.spawnArea.y;
		points[1].x = this.spawnArea.x + this.spawnArea.width;
		points[1].y = this.spawnArea.y;
		points[2].x = this.spawnArea.x + this.spawnArea.width;
		points[2].y = this.spawnArea.y + this.spawnArea.height;
		points[3].x = this.spawnArea.x;
		points[3].y = this.spawnArea.y + this.spawnArea.height;
		points[4].x = this.spawnArea.x;
		points[4].y = this.spawnArea.y;
	
		UnityEditor.Handles.DrawPolyLine(points);

		UnityEditor.Handles.color = original;
	}
#endif
}
