using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;

    [Header("Cube 3 data")]
    [SerializeField] float angleC3;


    Quaternion o3;

    private void Start()
    {

        //make a vector
        //Vector3 v = new Vector3(1, Mathf.Sqrt(2), -1.5f);
        Vector3 vz = Vector3.forward;
        Vector3 vx = Vector3.right;

        Quaternion o1 = cube1.transform.rotation;
        Quaternion o2 = cube2.transform.rotation;
        o3 = cube3.transform.rotation;

        Quaternion r = Quaternion.Euler(90, 90, 0);
        cube1.transform.rotation = r;
        Quaternion q = cube2.transform.rotation * r;
        cube2.transform.rotation = q;

        Vector3 axis;
        float angle3;
        r.ToAngleAxis(out angle3, out axis);
        Debug.Log($"angle3 = {angle3}");

        float angle1 = Quaternion.Angle(o1, cube1.transform.rotation);
        float angle2 = Quaternion.Angle(o2, cube2.transform.rotation);
        Debug.Log($"angle1 = {angle1}");
        Debug.Log($"angle2 = {angle2}");

    }

    private void Update()
    {
        angleC3 = Quaternion.Angle(o3, cube3.transform.rotation);
    }
}
