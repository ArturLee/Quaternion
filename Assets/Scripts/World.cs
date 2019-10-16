using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	public Mesh mesh;
	public Vector3[] vertices, initVertices;
	public int[] lines;
	public Vector3 point;

	void Start () {
		mesh = new Mesh();
		mesh.name = "Cube";
		GetComponent<MeshFilter>().mesh = mesh;

		point = new Vector3(10f, 0f, 0f);

		Vector3 p0 = new Vector3(-1f, -1f, -1f);
		Vector3 p1 = new Vector3(1f, -1f, -1f);
		Vector3 p2 = new Vector3(1f, -1f, -3f);
		Vector3 p3 = new Vector3(-1f, -1f, -3f);
		Vector3 p4 = new Vector3(-1f, 1f, -1f);
		Vector3 p5 = new Vector3(1f, 1f, -1f);
		Vector3 p6 = new Vector3(1f, 1f, -3f);
		Vector3 p7 = new Vector3(-1f, 1f, -3f);

		vertices = new Vector3[] {
			p0, p1, p2, p3, //Bottom face
			p7, p4, p0, p3, //Left face
			p4, p5, p1, p0, //Front face
			p6, p7, p3, p2, //Back face
			p5, p6, p2, p1, //Right face
			p7, p6, p5, p4 //Top face
		};

		lines = new int[] {
			0,1,
			0,3,
			0,5,
			1,2,
			1,9,
			2,3,
			2,12,
			3,4,
			5,9,
			5,4,
			9,12,
			12,4
		};

		mesh.vertices = vertices;		
	}
	public void Translate3D(float tx, float ty, float tz){
        Matrix4x4 translation_matrix = new Matrix4x4();
        translation_matrix.SetRow(0, new Vector4(1f, 0f, 0f, tx));
        translation_matrix.SetRow(1, new Vector4(0f, 1f, 0f, ty));
        translation_matrix.SetRow(2, new Vector4(0f, 0f, 1f, tz));
        translation_matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));

        for (int i = 0; i< vertices.Length; i++) {
            vertices[i] = translation_matrix.MultiplyPoint(vertices[i]);
        }
        mesh.vertices = vertices;
    }

	public void Scale3D(float sx, float sy, float sz){
        Matrix4x4 scale_matrix = new Matrix4x4();
        scale_matrix.SetRow(0, new Vector4(sx, 0f, 0f, 0));
        scale_matrix.SetRow(1, new Vector4(0f, sy, 0f, 0));
        scale_matrix.SetRow(2, new Vector4(0f, 0f, sz, 0));
        scale_matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        
		for (int i = 0; i< vertices.Length; i++) {
            vertices[i] = scale_matrix.MultiplyPoint(vertices[i]);
        }
        mesh.vertices = vertices;
    }

	public void RotateX3D(float angle) {
        angle = angle * Mathf.Deg2Rad;
        Matrix4x4 rotation_mat = new Matrix4x4();
        rotation_mat.SetRow(0, new Vector4(1f,0f,0f,0f));
        rotation_mat.SetRow(1, new Vector4(0f, Mathf.Cos(angle), -Mathf.Sin(angle), 0f));
        rotation_mat.SetRow(2, new Vector4(0f, Mathf.Sin(angle), Mathf.Cos(angle), 0f));
        rotation_mat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));

        for (int i = 0; i< vertices.Length; i++) {
            vertices[i] = rotation_mat.MultiplyPoint(vertices[i]);
        }
        mesh.vertices = vertices;
    }

    public void RotateY3D(float angle)
    {
        angle = angle * Mathf.Deg2Rad;
        Matrix4x4 rotation_mat = new Matrix4x4();
        rotation_mat.SetRow(0, new Vector4(Mathf.Cos(angle), 0f, Mathf.Sin(angle), 0f));
        rotation_mat.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
        rotation_mat.SetRow(2, new Vector4(-Mathf.Sin(angle), 0f, Mathf.Cos(angle), 0f));
        rotation_mat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rotation_mat.MultiplyPoint(vertices[i]);
        }
        mesh.vertices = vertices;
    }

    public void RotateZ3D(float angle)
    {
        angle = angle * Mathf.Deg2Rad;
        Matrix4x4 rotation_mat = new Matrix4x4();
        rotation_mat.SetRow(0, new Vector4(Mathf.Cos(angle), -Mathf.Sin(angle), 0f, 0f));
        rotation_mat.SetRow(1, new Vector4(Mathf.Sin(angle), Mathf.Cos(angle), 0f, 0f));
        rotation_mat.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
        rotation_mat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rotation_mat.MultiplyPoint(vertices[i]);
        }
        mesh.vertices = vertices;
    }

	public void RotateAroundCenter(float angle, string axis)
    {
        Matrix4x4 trans1 = new Matrix4x4();
        trans1.SetRow(0, new Vector4(1f, 0f, 0f, -point.x));
        trans1.SetRow(1, new Vector4(0f, 1f, 0f, -point.y));
        trans1.SetRow(2, new Vector4(0f, 0f, 1f, -point.z));
        trans1.SetRow(3, new Vector4(0f, 0f, 0f, 1f));

        Matrix4x4 rotation_mat = new Matrix4x4();
        angle = angle * Mathf.Deg2Rad;

        if (axis == "x") {
            rotation_mat.SetRow(0, new Vector4(1f,0f,0f,0f));
            rotation_mat.SetRow(1, new Vector4(0f, Mathf.Cos(angle), -Mathf.Sin(angle), 0f));
            rotation_mat.SetRow(2, new Vector4(0f, Mathf.Sin(angle), Mathf.Cos(angle), 0f));
            rotation_mat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        } else if (axis == "y") {
            rotation_mat.SetRow(0, new Vector4(Mathf.Cos(angle), 0f, Mathf.Sin(angle), 0f));
            rotation_mat.SetRow(1, new Vector4(0f, 1f, 0f, 0f));
            rotation_mat.SetRow(2, new Vector4(-Mathf.Sin(angle), 0f, Mathf.Cos(angle), 0f));
            rotation_mat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        } else {
            rotation_mat.SetRow(0, new Vector4(Mathf.Cos(angle), -Mathf.Sin(angle), 0f, 0f));
            rotation_mat.SetRow(1, new Vector4(Mathf.Sin(angle), Mathf.Cos(angle), 0f, 0f));
            rotation_mat.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
            rotation_mat.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        }

        Matrix4x4 trans2 = new Matrix4x4();
        trans2.SetRow(0, new Vector4(1f, 0f, 0f, point.x));
        trans2.SetRow(1, new Vector4(0f, 1f, 0f, point.y));
        trans2.SetRow(2, new Vector4(0f, 0f, 1f, point.z));
        trans2.SetRow(3, new Vector4(0f, 0f, 0f, 1f));

        Matrix4x4 final = (trans2 * rotation_mat) * trans1;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = rotation_mat.MultiplyPoint(vertices[i]);
        }
        mesh.vertices = vertices;
    }

	public void Reset() {
		Vector3 p0 = new Vector3(-1f, -1f, -1f);
		Vector3 p1 = new Vector3(1f, -1f, -1f);
		Vector3 p2 = new Vector3(1f, -1f, -3f);
		Vector3 p3 = new Vector3(-1f, -1f, -3f);
		Vector3 p4 = new Vector3(-1f, 1f, -1f);
		Vector3 p5 = new Vector3(1f, 1f, -1f);
		Vector3 p6 = new Vector3(1f, 1f, -3f);
		Vector3 p7 = new Vector3(-1f, 1f, -3f);

		vertices = new Vector3[] {
			p0, p1, p2, p3, //Bottom face
			p7, p4, p0, p3, //Left face
			p4, p5, p1, p0, //Front face
			p6, p7, p3, p2, //Back face
			p5, p6, p2, p1, //Right face
			p7, p6, p5, p4 //Top face
		};
	}
	
	void Update () {
		///RotateX3D(90);
        //RotateY3D(20 * Time.deltaTime);
        //RotateZ3D(-20 * Time.deltaTime);
	}
}
