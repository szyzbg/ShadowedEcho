using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjBScript : MonoBehaviour
{
    ObjAScript aScript;
    // Start is called before the first frame update
    void Start()
    {
        aScript = gameObject.AddComponent<ObjAScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
