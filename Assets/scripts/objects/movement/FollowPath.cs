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

	/**
	 * Draw the point in global coordinates
	 *
	 * @param  [ in]label A label used to indentify the point
	 */
	public void draw(string label) {
		this.draw(null, label);
	}

	/**
	 * Draw the point in "local coordinates"
	 *
	 * @param  [ in]initialPos The point's origin
	 * @param  [ in]label      A label used to indentify the point
	 */
	public void draw(PathPoint initialPos, string label) {
		Vector3 pos;

		pos = this.position;
		if (initialPos != null) {
			pos += initialPos.position;
		}

		UnityEditor.Handles.DrawSolidDisc(pos,
				Vector3.forward, PathPoint.pointRadius);
		UnityEditor.Handles.Label(pos + Vector3.right,
				"Time: " + this.time.ToString("F2"));
		UnityEditor.Handles.Label(pos + Vector3.right + Vector3.up * 0.2f,
				label);
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
		Rect rect;
		Vector3 pointPos;
		float pointTime;
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
	private PathPoint _current = null;
	/** Point between current and next */
	private PathPoint _intermediate = null;
	/** Next destination (one point ahead) */
	private PathPoint _next = null;

	/** Current time on the lerp */
	private float _time = 0.0f;

	/** Current position within the array */
	private int _index = 0;

	/** Called as soon as the gameObject is instantiated. Used to
	 * be sure '_current' is set and alloc'ed only once */
	void Awake() {
		this._current = new PathPoint();
	}

	/** Called when this is to be dealloc'ed. Releases all
	 * references and stuff */
	void OnDestroy() {
		int i;

		this.initialPosition = null;
		this.finalPosition = null;

		i = 0;
		while (i < this.points.Length) {
			this.points[i] = null;
			i++;
		}

		this.points = null;

		this._current = null;
		this._intermediate = null;
		this._next = null;
	}

	/** Called whenever the gameObject is (re-)enabled, so
	 * everything is properly initialized */
	void OnEnable() {
		this._index = 0;
		this._time = 0.0f;

		/* Current position is set to zero because the initial
		 * point (i.e., either point[0] or finalPosition) will
		 * be the offset from the initialPosition (i.e., the
		 * origin) */
		this._current.position = Vector3.zero;
		this.transform.position = this.initialPosition.position;
		if (this.points.Length > 0) {
			this.speed = 1 / this.points[0].time;
		}
		else {
			this.speed = 1 / this.finalPosition.time;
		}

		this.getNextPoints();
	}

	/** Sets the two following points to be targeted */
	private void getNextPoints() {
		if (this._index < this.points.Length - 1) {
			this._intermediate = this.points[this._index];
			this._next = this.points[this._index + 1];
		}
		else if (this._index < this.points.Length) {
			this._intermediate = this.points[this._index];
			this._next = this.finalPosition;
		}
		else {
			this._intermediate = this.finalPosition;
			this._next = this.finalPosition;
		}
	}

	/** Interpolate between this point and the next two ones */
	private Vector3 easySpline() {
		Vector3 res, v1, v2;
		float tn;

		/* Retrieve the position within the current section */
		tn = this._time / this._intermediate.time;

		/* Calculate the two vectors from the current node to the next
		 * and from the next to the one following it */
		v1 = this._intermediate.position - this._current.position;
		v2 = this._next.position - this._intermediate.position;

		/* Interpolate between both points so it starts targeting v1
		 * (at tn == 0), moves somewhat between both (at tn == 0.5)
		 * and targets v2 (at tn == 1.0). This is done so the movement
		 * is smoothed between sections */
		res = v1 * (3.0f - 2.0f * tn) + v2 * (1.0f + 2.0f * tn);
		res *= 0.25f;

		return res;
	}

	protected override void fixedUpdate () {
		Vector3 v;

		/* Get the interpolated velocity vector */
		v = this.easySpline();
		this.velocity = new Vector2(v.x, v.y);

		/* Check if we reached the current target and move to the next one */
		this._time += Time.fixedDeltaTime;
		if (this._time >= this._intermediate.time) {
			this._time -= this._intermediate.time;

			/* Update the current position so the component
			 * accounts for error */
			this._current.position = this.transform.position;
			this._current.position -= this.initialPosition.position;
			this.speed = 1 / this._next.time;

			this._index++;
			this.getNextPoints();
		}
	}

	/* Draw the path and the points on the editor */
	void OnDrawGizmos() {
		Color original;
		Vector3 lastPos, nextPos;
		int i;

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
		this.initialPosition.draw("initial");

		UnityEditor.Handles.color = this.pointColor;
		i = 0;
		foreach (PathPoint p in this.points) {
			p.draw(this.initialPosition, "Point " + i.ToString());
			i++;
		}

		UnityEditor.Handles.color = this.finalPositionColor;
		this.finalPosition.draw(this.initialPosition, "final");

		UnityEditor.Handles.color = original;
	}
}
