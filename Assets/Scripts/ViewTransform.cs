using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewTransform : MonoBehaviour {

	public World world;

	public Toggle toggle;

	public Toggle ortho;

	private float left_plane = -5f;
	private float right_plane = 5f;
	private float bottom_plane = -5f;
	private float top_plane = 5f;
	private float near_plane = -1f;
	private float far_plane = -11f;

	public Vector3 eye;
	public Vector3 gaze;
	public Vector3 up;

	public float fovAngle = 100;

	public float aspect;

	void OnGUI () {
		if (toggle.isOn) {
			if (ortho.isOn) {
				Matrix4x4 mvp = new Matrix4x4();
				mvp.SetRow(0,new Vector4(Screen.width/2f,0f,0f,(Screen.width-1)/2f));
				mvp.SetRow(1,new Vector4(0f,Screen.height/2f,0f,(Screen.height-1)/2f));
				mvp.SetRow(2,new Vector4(0f, 0f, 1f, 0f));
				mvp.SetRow(3,new Vector4(0f, 0f, 0f, 1f));
				Matrix4x4 morth = new Matrix4x4();
				morth.SetRow(0, new Vector4(2f / (right_plane - left_plane), 0f, 0f,
				-((right_plane+left_plane)/(right_plane-left_plane))));
				morth.SetRow(1, new Vector4(0f, 2f / (top_plane - bottom_plane), 0f,
				-((top_plane + bottom_plane) / (top_plane - bottom_plane))));
				morth.SetRow(2, new Vector4(0f, 0f, 2f / (near_plane - far_plane),
				-((near_plane + far_plane) / (near_plane - far_plane))));
				morth.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
				Matrix4x4 m = mvp * morth;

				for (int i = 0; i < world.lines.Length; i+=2) {
					Vector4 p = multiplyPoint(m,
					new Vector4(world.vertices[world.lines[i]].x,
					world.vertices[world.lines[i]].y,
					world.vertices[world.lines[i]].z, 1));
					Vector4 q = multiplyPoint(m,
					new Vector4(world.vertices[world.lines[i + 1]].x,
					world.vertices[world.lines[i + 1]].y,
					world.vertices[world.lines[i + 1]].z, 1));
					GuiHelper.DrawLine(new Vector2(p.x, p.y), new Vector2(q.x, q.y),
					Color.black);
				}


			} else {
				Vector3 w = -gaze.normalized;
				Vector3 u = Vector3.Cross(up, w).normalized;
				Vector3 v = Vector3.Cross(w, u);

				Matrix4x4 orthoMatrix = new Matrix4x4();
				orthoMatrix.SetRow(0, new Vector4(2 / (right_plane - left_plane), 0, 0, -((right_plane + left_plane) / (right_plane - left_plane))));
				orthoMatrix.SetRow(1, new Vector4(0, 2 / (top_plane - bottom_plane), 0, -((top_plane + bottom_plane) / (top_plane - bottom_plane))));
				orthoMatrix.SetRow(2, new Vector4(0, 0, 2 / (near_plane - far_plane), -((near_plane + far_plane) / (near_plane - far_plane))));
				orthoMatrix.SetRow(3, new Vector4(0, 0, 0, 1));

				Matrix4x4 mvp = new Matrix4x4();
				mvp.SetRow(0, new Vector4(Screen.width/2, 0, 0, (Screen.width - 1) / 2));
				mvp.SetRow(1, new Vector4(0, Screen.height/2, 0, (Screen.height - 1) / 2));
				mvp.SetRow(2, new Vector4(0, 0, 1, 0));
				mvp.SetRow(3, new Vector4(0, 0, 0, 1));

				Matrix4x4 mcam = new Matrix4x4();
				mcam.SetRow(0, new Vector4(u.x, u.y, u.z, -((u.x * eye.x) + (u.y * eye.y) + (u.z * eye.z))));
				mcam.SetRow(1, new Vector4(v.x, v.y, v.z, -((v.x * eye.x) + (v.y * eye.y) + (v.z * eye.z))));
				mcam.SetRow(2, new Vector4(w.x, w.y, w.z, -((w.x * eye.x) + (w.y * eye.y) + (w.z * eye.z))));
				mcam.SetRow(3, new Vector4(0, 0, 0, 1));

				UpdateViewVolume(eye);

				Matrix4x4 mper = new Matrix4x4();
				mper.SetRow(0, new Vector4((2 * near_plane) / (right_plane - left_plane), 0, (left_plane + right_plane) / (left_plane - right_plane), 0));
				mper.SetRow(1, new Vector4(0, (2 * near_plane) / (top_plane - bottom_plane), (bottom_plane + top_plane) / (bottom_plane - top_plane), 0));
				mper.SetRow(2, new Vector4(0, 0, (far_plane + near_plane) / (near_plane - far_plane), (2 * far_plane * near_plane) / (far_plane - near_plane)));
				mper.SetRow(3, new Vector4(0, 0, 1, 0));

				float angle = fovAngle * Mathf.Deg2Rad;

				Matrix4x4 mper2 = new Matrix4x4();
				mper2.SetRow(0, new Vector4(1/(Mathf.Tan(angle) * aspect), 0, 0, 0));
				mper2.SetRow(1, new Vector4(0, 1/Mathf.Tan(angle), 0, 0));
				mper2.SetRow(2, new Vector4(0, 0, (far_plane + near_plane)/(near_plane - far_plane),  (2 * far_plane * near_plane) / (far_plane - near_plane)));
				mper2.SetRow(3, new Vector4(0, 0, 1, 0));


				//Matrix4x4 final = mvp * ( mper * mcam);
				Matrix4x4 final = mvp * ( mper2  * mcam );

				for (int i = 0; i < world.lines.Length; i+=2) {
					Vector4 p = multiplyPoint(final, new Vector4(world.vertices[world.lines[i]].x,world.vertices[world.lines[i]].y,world.vertices[world.lines[i]].z, 1));
					Vector4 q = multiplyPoint(final, new Vector4(world.vertices[world.lines[i + 1]].x,world.vertices[world.lines[i + 1]].y,world.vertices[world.lines[i + 1]].z, 1));
					GuiHelper.DrawLine(new Vector2(p.x/p.w, p.y/p.w), new Vector2(q.x/q.w, q.y/q.w), Color.black);
				}
			}
		}
	}

	void UpdateViewVolume(Vector3 e){
		near_plane = e.z - 3;
		far_plane = e.z - 13;
		right_plane = e.x - 5;
		left_plane = e.x + 5;
		top_plane = e.y + 5;
		bottom_plane = e.y - 5;
	}
	
	Vector4 multiplyPoint(Matrix4x4 matrix, Vector4 point) {
		Vector4 result = new Vector4();
		for (int r = 0; r < 4; r++){
			float s = 0;
			for(int z = 0; z < 4; z++){
				s += matrix[r,z] * point[z];
			}
			result[r] = s;
		}
		return result;
	}
	void Update () {
		
	}
}
