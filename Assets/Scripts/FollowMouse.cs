using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

    public Camera playerCam;

	// Update is called once per frame
	private void Update () {
        // transform.localEulerAngles = new Vector3(playerCam.transform.localEulerAngles.x + 60, 0, 0);
        // transform.position = new Vector3(transform.parent.position.x + 0.5f, transform.parent.position.y + 1.0f + playerCam.transform.localEulerAngles.x * -1 / 100.0f, transform.parent.position.z);
    }
}
