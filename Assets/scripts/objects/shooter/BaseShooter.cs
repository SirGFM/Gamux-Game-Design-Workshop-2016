using Color = UnityEngine.Color;
using MonoBehaviour = UnityEngine.MonoBehaviour;
using Vector3 = UnityEngine.Vector3;
#if UNITY_EDITOR
using Handles = UnityEditor.Handles;
#endif

public abstract class BaseShooter : MonoBehaviour {

	/** Radius of the offset */
	public const float radius = 0.05f;

	/** Color of the hitbox */
	public Color color = Color.white;
	
	/** Offset of the hitbox from the gameObject's center */
	public Vector3 offset;

	/** Number of bullets shot at once */
	public int count = 1;

	/** Recycler used to spawn bullets */
	public Recycler recycler;

	/** Opening angle, used when shooting multiple
	 * bullets toward the forward angle */
	public float angleRange = 60.0f;

	/** The movement component */
	protected BaseMovement move;

	/**
	 * Override this function to implement the component's
	 * shooting behaviour. It then should be called by the
	 * component used to control *when* to shoot.
	 *
	 * @param  [ in]angle Direction toward which bullets
	 *                    should be shot (0º is to the rigth, 90º is upward)
	 */
	public abstract void shoot(float angle);

	void Start() {
		this.move = this.GetComponent<BaseMovement>();
	}

#if UNITY_EDITOR
	/* Draw the actual hitbox on the editor */
	void OnDrawGizmos() {
		Color original;

		original = UnityEditor.Handles.color;
		Handles.color = this.color;

		Handles.DrawWireDisc(this.transform.position + this.offset,
				Vector3.back, BaseShooter.radius);

		Handles.color = original;
	}
#endif
}
