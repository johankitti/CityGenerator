using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

    [Tooltip("From 0-100 How common?")]
    public int HowCommon = 100;

    private float m_Height;
    public float Height {
        get { return m_Height; }
    }

	void Start () {
        m_Height = CalcBuildingHeight();
    }

    float CalcBuildingHeight() {
        return GetComponentInChildren<MeshFilter>().mesh.bounds.size.y;
    }
}
