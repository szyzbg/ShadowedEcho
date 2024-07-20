using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjCScript : MonoBehaviour
{
    GameObject aObject;
    ObjBScript aScript;
    // Start is called before the first frame update
    void Start()
    {
        aObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        aObject.AddComponent<ObjBScript>();
        aScript = GameObject.Find("Obj-B").GetComponent<ObjBScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
