using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPerson : MonoBehaviour {

	public bool lockCursor;
	public float gravity = 12f;
	public float jumpForce = 20;
	public float moveSpeed = 5;
	public Vector2 mouseSensitivity;
	public Vector2 verticalLookMinMax;
	public Transform cam;
	CharacterController controller;
	float pitch;
	float velocityY;
	Vector3 dirOld;

	void Start () {
		controller = GetComponent<CharacterController> ();

		if (lockCursor) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
	

	void Update () {
		Vector3 moveDir = new Vector3 (Input.GetAxisRaw ("Horizontal"),0,Input.GetAxisRaw ("Vertical")).normalized;
		Vector2 mouseInput = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));

		if (controller.isGrounded) {
			velocityY = 0;

			if (Input.GetKeyDown (KeyCode.Space)) {
				velocityY = jumpForce;
			}
		} else {
			moveDir = dirOld;
		}

		transform.Rotate (Vector3.up * mouseInput.x * mouseSensitivity.x);
		pitch += mouseInput.y * mouseSensitivity.y;
		pitch = ClampAngle (pitch, verticalLookMinMax.x, verticalLookMinMax.y);
		Quaternion yQuaternion = Quaternion.AngleAxis (pitch, Vector3.left);
		cam.localRotation =  yQuaternion;

		velocityY -= gravity * Time.deltaTime;
		Vector3 velocity = transform.TransformDirection(moveDir) * moveSpeed + Vector3.up * velocityY;
		controller.Move (velocity * Time.deltaTime);
		dirOld = moveDir;
	
	}

	static float ClampAngle (float angle, float min, float max) {
		if (angle < -360f)
			angle += 360f;
		if (angle > 360f)
			angle -= 360f;
		return Mathf.Clamp (angle, min, max);
	}
}
