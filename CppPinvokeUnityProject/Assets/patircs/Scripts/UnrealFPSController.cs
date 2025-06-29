using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Camera))]
public class UnrealFPSController: MonoBehaviour {
	[SerializeField] float _mouse_sensitivity = 100f;
	[SerializeField] float _move_speed = 40f;

	Camera _camera;

	void Awake()
	{
		_camera = GetComponent<Camera>();
		Assert.IsNotNull(_camera, "");
	}

	void OnDestroy()
	{
	}

	void Update()
	{
		float h, v, u;
		if (Input.GetMouseButton(1)) {
			Cursor.visible = false;
			h = Input.GetAxisRaw("Mouse X");
			v = Input.GetAxisRaw("Mouse Y");
			Vector3 f = new Vector3(-v, h, 0f);
			f *= _mouse_sensitivity * Time.deltaTime;

			_camera.transform.Rotate(f);
			Vector3 a = _camera.transform.eulerAngles;
			a.z = 0f;
			_camera.transform.eulerAngles = a;


			h = Input.GetAxisRaw("Horizontal");
			v = Input.GetAxisRaw("Vertical");
			u = Input.GetAxisRaw("UpDown");
			Vector3 d = new Vector3(h, 0f, v);
			d.Normalize();
			d *= Time.deltaTime * _move_speed;
			_camera.transform.Translate(d, Space.Self);
			d = new Vector3(0f, u, 0f);
			d.Normalize();
			d *= Time.deltaTime * _move_speed;
			_camera.transform.Translate(d, Space.World);

		} else {
			Cursor.visible = true;
		}

	}
}

