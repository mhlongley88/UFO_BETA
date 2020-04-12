using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableMeshRenderer : MonoBehaviour
{

    public MeshRenderer myMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myMeshRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
