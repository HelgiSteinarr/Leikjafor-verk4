using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraMouseLook : MonoBehaviour {

    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;
    public GameObject character;
    public Rigidbody charactedRigidBody;
    public GameObject gun;

    private Vector2 mouseLook;
    private Vector2 smoothV;

    private void Start () {
        
	}
	
	private void Update () {
        if (!transform.parent.GetComponent<CharacterController>().isLocalPlayer) return;

        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1 / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1 / smoothing);
        mouseLook += smoothV;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        charactedRigidBody.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, charactedRigidBody.transform.up);
    }
}
