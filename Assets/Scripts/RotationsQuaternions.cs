using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationsQuaternions : MonoBehaviour {

	public Slider translateSliderX, translateSliderY, translateSliderZ;
    public Slider rotateSliderX, rotateSliderY, rotateSliderZ;
	public Slider scaleSliderX, scaleSliderY, scaleSliderZ;
    public Slider rotateQuaternSliderW, rotateQuaternSliderX, rotateQuaternSliderY, rotateQuaternSliderZ;
    public Toggle quaternToggle;

    public Toggle carToggle, deerToggle;

    public Text txl, tyl, tzl, rxl, ryl, rzl, sxl, syl, szl;

	public float translateVectorX = 0;
	public float translateVectorY = 0;
	public float translateVectorZ = 0;

    public Transform car;
    public Transform cube;

    private Transform activeModel;
    private Matrix4x4 carTRS;
    private World cubeWorld;
    private Quaternion carRotation, deerRotation;

    public Transform cubePanel, carPanel;

    bool isQuatern = false;
    
    void Start() {
        translateSliderX.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
        translateSliderY.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
        translateSliderZ.onValueChanged.AddListener(delegate {ValueChangeCheck(); });

        rotateSliderX.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
        rotateSliderY.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
        rotateSliderZ.onValueChanged.AddListener(delegate {ValueChangeCheck(); });

		scaleSliderX.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
		scaleSliderY.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
		scaleSliderZ.onValueChanged.AddListener(delegate {ValueChangeCheck(); });

        rotateQuaternSliderW.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
		rotateQuaternSliderX.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
		rotateQuaternSliderY.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
		rotateQuaternSliderZ.onValueChanged.AddListener(delegate {ValueChangeCheck(); });

        quaternToggle.onValueChanged.AddListener(delegate {ValueChangeCheck(); });

        carToggle.onValueChanged.AddListener(delegate {ValueChangeCheck(); });
        deerToggle.onValueChanged.AddListener(delegate {ValueChangeCheck(); });

        cubeWorld = cube.GetComponent<World>();
        deerRotation = cube.rotation;
        Debug.Log(deerRotation);

        carTRS = Matrix4x4.TRS(car.transform.position,car.transform.rotation,car.transform.localScale);
        carRotation = car.rotation;
        activeModel = car;

        ValueChangeCheck();

        Debug.Log(Euler(46,50,10));
        Debug.Log(Quaternion.Euler(46,50,10));
	}

	void Update() {
		//Translate3D(0,2 * Time.deltaTime,0);
		//Scale3D(2,0,0);
		//Rotate3D(-10 * Time.deltaTime,10 * Time.deltaTime,-10 * Time.deltaTime);
	}

    public Vector4 Quatern(float w, float x, float y, float z) {
		float radAngle = w * Mathf.Deg2Rad;
		float w_ = Mathf.Cos(radAngle/2);
		float x_ = Mathf.Sin(radAngle/2) * x;
		float y_ = Mathf.Sin(radAngle/2) * y;
		float z_ = Mathf.Sin(radAngle/2) * z;
		return new Vector4(w_, x_, y_, z_);
	}

	public Vector4 InverseQuatern(float w, float x, float y, float z) {
		float radAngle = w * Mathf.Deg2Rad;
		float w_ = Mathf.Cos(radAngle/2);
		float x_ = Mathf.Sin(radAngle/2) * -x;
		float y_ = Mathf.Sin(radAngle/2) * -y;
		float z_ = Mathf.Sin(radAngle/2) * -z;
		return new Vector4(w_, x_, y_, z_);
	}

	public Vector4 MultQuatern(float w, Vector3 vec, float w2, Vector3 vec2) {
		float radAngle = w * Mathf.Deg2Rad;
		float wr = Mathf.Cos(radAngle/2);
		float x_ = Mathf.Sin(radAngle/2) * vec.x;
		float y_ = Mathf.Sin(radAngle/2) * vec.y;
		float z_ = Mathf.Sin(radAngle/2) * vec.z;
		Vector3 vr = new Vector3(x_,y_,z_);

		float radAngle2 = w2 * Mathf.Deg2Rad;
		float ws = Mathf.Cos(radAngle2/2);
		float x2_ = Mathf.Sin(radAngle2/2) * vec2.x;
		float y2_ = Mathf.Sin(radAngle2/2) * vec2.y;
		float z2_ = Mathf.Sin(radAngle2/2) * vec2.z;
		Vector3 vs = new Vector3(x2_,y2_,z2_);

		float out1 = (ws * wr) + Vector3.Dot(vs,vr);
		Vector3 out2 = (ws * vr) + (wr * vs) + Vector3.Cross(vr, vs);
		float result = (out1 * out1) + (out2.x * out2.x) + (out2.y * out2.y) + (out2.z * out2.z);
		return new Vector4(out1, out2.x, out2.y, out2.z);
	}

    public static Quaternion Euler(float yaw, float pitch, float roll) {
        yaw*=Mathf.Deg2Rad;
        pitch*=Mathf.Deg2Rad;
        roll*=Mathf.Deg2Rad;

        double yawOver2 = yaw * 0.5f;
        float cosYawOver2 = (float)System.Math.Cos(yawOver2);
        float sinYawOver2 = (float)System.Math.Sin(yawOver2);
        double pitchOver2 = pitch * 0.5f;
        float cosPitchOver2 = (float)System.Math.Cos(pitchOver2);
        float sinPitchOver2 = (float)System.Math.Sin(pitchOver2);
        double rollOver2 = roll * 0.5f;
        float cosRollOver2 = (float)System.Math.Cos(rollOver2);
        float sinRollOver2 = (float)System.Math.Sin(rollOver2);            
        Quaternion result;
        result.w = cosYawOver2 * cosPitchOver2 * cosRollOver2 + (sinYawOver2 * sinPitchOver2 * sinRollOver2);
        result.x = sinYawOver2 * cosPitchOver2 * cosRollOver2 + (cosYawOver2 * sinPitchOver2 * sinRollOver2);
        result.y = cosYawOver2 * sinPitchOver2 * cosRollOver2 - (sinYawOver2 * cosPitchOver2 * sinRollOver2);
        result.z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - (sinYawOver2 * sinPitchOver2 * cosRollOver2);

        return result;
    }   

    void RotateQuatern(float w, Vector3 vec) {
        if(activeModel == car) {
            Vector4 mult = MultQuatern(carRotation.w, new Vector3(carRotation.x, carRotation.y, carRotation.z), w, vec);
            car.rotation = new Quaternion(mult.w,mult.x,mult.y,mult.z);
        } else {
            Vector4 mult = MultQuatern(deerRotation.w, new Vector3(deerRotation.x, deerRotation.y, deerRotation.z), w, vec);
            cube.rotation = new Quaternion(mult.w,mult.x,mult.y,mult.z);
        }        
    }


	//--------------------// MATRICES //--------------------//--------------------//------------------//

    void Translate3D(float tx, float ty, float tz){
        Matrix4x4 translation_matrix = new Matrix4x4();
        translation_matrix.SetRow(0, new Vector4(1f, 0f, 0f, tx));
        translation_matrix.SetRow(1, new Vector4(0f, 1f, 0f, ty));
        translation_matrix.SetRow(2, new Vector4(0f, 0f, 1f, tz));
        translation_matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        if(activeModel == car) {
            Matrix4x4 final = carTRS * translation_matrix;
            car.transform.position = -ExtractPosition(final);
        } else {
            cubeWorld.Translate3D(tx, ty, tz);
        }
    }

    void Scale3D(float sx, float sy, float sz){
        Matrix4x4 scale_matrix = new Matrix4x4();
        scale_matrix.SetRow(0, new Vector4(sx, 0f, 0f, 0));
        scale_matrix.SetRow(1, new Vector4(0f, sy, 0f, 0));
        scale_matrix.SetRow(2, new Vector4(0f, 0f, sz, 0));
        scale_matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
        if(activeModel == car) {
            Matrix4x4 final = carTRS * scale_matrix;
            car.transform.localScale = ExtractScale(final);
        } else {
            cubeWorld.Scale3D(sx, sy, sz);
        }
        
    }

	void Rotate3D(float xangle,float yangle, float zangle) {
        if(activeModel == car) {
            Matrix4x4 final = RotateX3D(xangle) * RotateY3D(yangle) * RotateZ3D(zangle);
            car.transform.rotation = ExtractRotation(final);
        } else {
            //cubeWorld.RotateX3D(xangle);
            //cubeWorld.RotateY3D(yangle);
            //cubeWorld.RotateZ3D(zangle);

            cubeWorld.RotateAroundCenter(xangle, "x");
            cubeWorld.RotateAroundCenter(yangle, "y");
            cubeWorld.RotateAroundCenter(zangle, "z");
        }
		
	}

    /*void RotateAroundPoint3DVert(float angle, Vector3 point, string axis)
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

        for (int i = 0; i < cubeVertices.Length; i++)
        {
            cubeVertices[i] = final.MultiplyPoint(cubeVerticesInit[i]);
            //normals[i] = rotation_mat.MultiplyPoint(normals[i]);
        }
            cube.GetComponent<World>().applyVertices(cubeVertices);
        //mesh.normals = normals;
    }*/

    Matrix4x4 RotateX3D(float angle) {
        angle = angle * Mathf.Deg2Rad;
        Matrix4x4 rxmat = new Matrix4x4();
        rxmat.SetRow(0,new Vector4(1f,0f, 0f, 0f));
        rxmat.SetRow(1,new Vector4(0f,Mathf.Cos(angle),-Mathf.Sin(angle),0f));
        rxmat.SetRow(2,new Vector4(0f,Mathf.Sin(angle),Mathf.Cos(angle), 0f));
        rxmat.SetRow(3,new Vector4(0f,0f, 0f, 1f));
        return carTRS * rxmat;
    }

    Matrix4x4 RotateY3D(float angle) {
        angle = angle * Mathf.Deg2Rad;
        Matrix4x4 rymat = new Matrix4x4();
        rymat.SetRow(0,new Vector4(Mathf.Cos(angle), 0f,Mathf.Sin(angle),0f));
        rymat.SetRow(1,new Vector4(0f, 1f,0f, 0f));
        rymat.SetRow(2,new Vector4(-Mathf.Sin(angle),0f,Mathf.Cos(angle),0f));
        rymat.SetRow(3,new Vector4(0f, 0f,0f, 1f));
        return carTRS * rymat;
    }
    
    Matrix4x4 RotateZ3D(float angle) {
        angle = angle * Mathf.Deg2Rad;
        Matrix4x4 rzmat = new Matrix4x4();
        rzmat.SetRow(0,new Vector4(Mathf.Cos(angle),-Mathf.Sin(angle),0f,0f));
        rzmat.SetRow(1,new Vector4(Mathf.Sin(angle), Mathf.Cos(angle),0f,0f));
        rzmat.SetRow(2,new Vector4(0f, 0f, 1f,0f));
        rzmat.SetRow(3,new Vector4(0f, 0f, 0f,1f));
        return carTRS * rzmat;
    }

    Quaternion ExtractRotation(Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;
 
        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;
 
        return Quaternion.LookRotation(forward, upwards);
    }
 
    Vector3 ExtractPosition(Matrix4x4 matrix)
    {
        Vector3 position;
        position.x = matrix.m03;
        position.y = matrix.m13;
        position.z = matrix.m23;
        return position;
    }
 
    Vector3 ExtractScale(Matrix4x4 matrix)
    {
        Vector3 scale;
        scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
        scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
        scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
        return scale;
    }

    public void ValueChangeCheck()
    {
        if (!isQuatern) {
            //cube.GetComponent<World>().applyVertices(cubeVerticesInit);
            //cubeVertices = cube.GetComponent<MeshFilter>().mesh.vertices;
            cubeWorld.Reset();
        } else {
            //cube.GetComponent<MeshFilter>().mesh.vertices = cubeVerticesInit;
        }

        if(carToggle.isOn && activeModel != car) {
            activeModel = car;
            car.gameObject.SetActive(true);
            cube.gameObject.SetActive(false);
            cubePanel.gameObject.SetActive(false);
            carPanel.gameObject.SetActive(true);
        } else if(deerToggle.isOn && activeModel != cube) {
            activeModel = cube;
            car.gameObject.SetActive(false);
            cube.gameObject.SetActive(true);
            cubePanel.gameObject.SetActive(true);
            carPanel.gameObject.SetActive(false);
        }

        txl.text = translateSliderX.value.ToString();
        tyl.text = translateSliderY.value.ToString();
        tzl.text = translateSliderZ.value.ToString();

        rxl.text = rotateSliderX.value.ToString();
        ryl.text = rotateSliderY.value.ToString();
        rzl.text = rotateSliderZ.value.ToString();
        
        sxl.text = scaleSliderX.value.ToString();
        syl.text = scaleSliderY.value.ToString();
        szl.text = scaleSliderZ.value.ToString();

        isQuatern = quaternToggle.isOn;
        translateVectorX = translateSliderX.value;
        translateVectorY = translateSliderY.value;
        translateVectorZ = translateSliderZ.value;

		Translate3D(translateSliderX.value, translateSliderY.value, translateSliderZ.value);
		Scale3D(scaleSliderX.value, scaleSliderY.value, scaleSliderZ.value);

        if (isQuatern) {
            RotateQuatern(rotateQuaternSliderW.value, new Vector3(rotateQuaternSliderX.value, rotateQuaternSliderY.value, rotateQuaternSliderZ.value));
        } else {
            Rotate3D(rotateSliderX.value, rotateSliderY.value, rotateSliderZ.value);
        }
    }
}
