using UnityEditor;
using UnityEngine;
using System.Collections;


/** Simple class to enable creating the path */
[System.Serializable]
public class PathPoint {

	/** Radius of a point */
	private const float pointRadius = 0.1f;

	/** Position of the point (offset from the object's
		 * initial position!) */
	public Vector3 position = Vector3.zero;

	/** How long should take to go from the previous
		 * point to this one */
	public float time = 1.0f;

	/** Draw the point in global coordinates */
	public void draw() {
		this.draw(null);
	}

	/**
	 * Draw the point in "local coordinates"
	 *
	 * @param  [ in]initialPos The point's origin
	 */
	public void draw(PathPoint initialPos) {
		Vector3 pos;

		pos = this.position;
		if (initialPos != null) {
			pos += initialPos.position;
		}

		UnityEditor.Handles.DrawSolidDisc(pos,
				Vector3.forward, PathPoint.pointRadius);
		UnityEditor.Handles.Label(pos + Vector3.right,
				"Time: " + this.time.ToString("F2"));
	}
}

[CustomPropertyDrawer(typeof(PathPoint))]
public class PathPointEditor : PropertyDrawer {
	/**
	 * Custom drawer for path point property
	 *
	 * @param  [ in]pos   Position within the inspecto's GUI
	 * @param  [ in]prop  
	 * @param  [ in]label The property's label
	 */
	override public void OnGUI(Rect pos, SerializedProperty prop, GUIContent label) {
		Rect rect, newrect;
		Vector3 pointPos;
		float pointTime, height;
		int indent;

		/* Retrieve the property's initial value */
		pointPos = prop.FindPropertyRelative("position").vector3Value;
		pointTime = prop.FindPropertyRelative("time").floatValue;

		EditorGUI.BeginProperty(pos, label, prop);

		rect = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);
		indent = EditorGUI.indentLevel;

		/* Clear the indent level so it looks nice even when nested */
		EditorGUI.indentLevel = 0;

		/* Set the width for the label and the field */
		EditorGUIUtility.labelWidth = 15.0f;
		EditorGUIUtility.fieldWidth = 45.0f;
		rect.width = EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth;

		/* Enable polling if the value was changed */
		EditorGUI.BeginChangeCheck();

		pointPos.x = EditorGUI.FloatField(rect, "x", pointPos.x);
		rect.x += rect.width + 5.0f;
		pointPos.y = EditorGUI.FloatField(rect, "y", pointPos.y);
		rect.x += rect.width + 5.0f;

		/* Update only if changed */
		if (EditorGUI.EndChangeCheck()) {
			prop.FindPropertyRelative("position").vector3Value = pointPos;
		}

		/* Modify the label width, since the 'time' label is longer */
		EditorGUIUtility.labelWidth = 40.0f;
		rect.width = EditorGUIUtility.labelWidth + EditorGUIUtility.fieldWidth;

		EditorGUI.BeginChangeCheck();
		pointTime = EditorGUI.FloatField(rect, "time", pointTime);
		if (EditorGUI.EndChangeCheck()) {
			prop.FindPropertyRelative("time").floatValue = pointTime;
		}

		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}

public class FollowPath : BaseMovement {

	/** Color of the initial position */
	public Color initialPositionColor = Color.white;

	/** Color of the final position */
	public Color finalPositionColor = Color.white;

	/** Color of the points belonging to the path */
	public Color pointColor = Color.white;

	/** Color of the line */
	public Color lineColor = Color.white;

	/** Initial position. It affects the path as soon as it's changed! */
	public PathPoint initialPosition;

	/** List of points between the initial and the final */
	public PathPoint []points;

	/** Final position. It's offset from the initial position! */
	public PathPoint finalPosition;

	/** First of the 3 points used to lerp */
	private PathPoint current = null;
	/** Point between current and next */
	private PathPoint intermediate = null;
	/** Next destination (one point ahead) */
	private PathPoint next = null;

	/** Current time on the lerp */
	private float t = 0.0f;

	/** Current position within the array */
	private int i = 0;

	protected Vector3 easySpline() {
		Vector3 res, v1, v2;
		float tn;

		//tn = this.t / this.intermediate.time;
		//v1 = this.intermediate.position * 3.0f * (1.0f - tn) + this.intermediate.position * tn;
		//v2 = this.next.position * (1.0f - tn) + this.next.position * 3.0f * tn ;
		//res = (v1 + v2) * 0.5f;
		res = this.intermediate.position - this.current.position;

		return res;
	}

	void Awake() {
		this.current = new PathPoint();
	}

	void OnEnable() {
		this.i = 0;
		this.t = 0.0f;

		this.current.position = Vector3.zero;
		this.current.time = 0.0f;
		this.transform.position = this.initialPosition.position;
	}

	protected override void fixedUpdate () {
		Vector3 v;

		if (this.i < this.points.Length - 1) {
			this.intermediate = this.points[this.i];
			this.next = this.points[this.i + 1];
		}
		else {
			this.intermediate = null;
			this.next = this.finalPosition;
		}

		v = this.easySpline();
		this.velocity = new Vector2(v.x, v.y) / this.intermediate.time;

		this.t += Time.fixedDeltaTime;
		if (this.t >= this.intermediate.time) {
			this.t -= this.intermediate.time;

			this.current.position = this.transform.position;
			this.current.time = this.next.time;

			this.i++;
		}
	}

	/* Draw the path and the points on the editor */
	void OnDrawGizmos() {
		Color original;
		Vector3 lastPos, nextPos;

		original = UnityEditor.Handles.color;

		/* Draw the path */
		UnityEditor.Handles.color = this.lineColor;
		lastPos = this.initialPosition.position;
		foreach(PathPoint next in this.points) {
			nextPos = next.position + this.initialPosition.position;
			UnityEditor.Handles.DrawLine(lastPos, nextPos);
			lastPos = nextPos;
		}
		nextPos = this.finalPosition.position;
		nextPos += this.initialPosition.position;
		UnityEditor.Handles.DrawLine(lastPos, nextPos);

		/* Draw all the points */
		UnityEditor.Handles.color = this.initialPositionColor;
		this.initialPosition.draw();

		UnityEditor.Handles.color = this.pointColor;
		foreach (PathPoint p in this.points) {
			p.draw(this.initialPosition);
		}

		UnityEditor.Handles.color = this.finalPositionColor;
		this.finalPosition.draw(this.initialPosition);

		UnityEditor.Handles.color = original;
	}
}
