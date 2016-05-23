using Input = UnityEngine.Input;

public class PlayerController : BaseController {

	/** Virtual button used to firing */
	public string fireButton = "Fire1";

	protected override bool isShooting () {
		return Input.GetButton(this.fireButton);
	}
}
