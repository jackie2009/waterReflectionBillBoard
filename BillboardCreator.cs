using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCreator : MonoBehaviour
{
    public Renderer []billboardRenders;
    public RenderTexture reflectRT;
    private int count ;
    public RenderTexture[] billboardTexs;

    private Camera cmr;

    private bool inited = false;
    // Start is called before the first frame update
    void Start()
    {
          cmr = GetComponent<Camera>();
        cmr.enabled = false;
        count = billboardRenders.Length;
        foreach (var b in billboardRenders)
        {
                  b.enabled = false;
        }
  
        billboardTexs=new RenderTexture[count];
        for (int i = 0; i < count; i++)
        {
            billboardTexs[i] = RenderTexture.GetTemporary(512, 512,16,RenderTextureFormat.ARGB32);
            cmr.targetTexture = billboardTexs[i];
            cmr.Render();
            transform.parent.Rotate(0,360.0f/count,0);
            billboardRenders[i].material=new Material(Shader.Find("Unlit/Transparent Cutout"));
            billboardRenders[i].material.mainTexture = billboardTexs[i];  
        } 
        foreach (var b in billboardRenders)
        {
                    b.enabled = true;
        }

        
        
        reflectRT = RenderTexture.GetTemporary(Camera.main.pixelWidth, Camera.main.pixelHeight, 16, RenderTextureFormat.ARGB32);
        Shader.SetGlobalTexture("waterReflectBDTex",reflectRT);
        cmr.targetTexture =reflectRT;
        cmr.enabled = true;
      
        cmr.cullingMask = LayerMask.GetMask("Water");
        inited = true;
        useBillboardMode(true);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < count; i++)
        {
             RenderTexture.ReleaseTemporary(billboardTexs[i]);
        }  
        RenderTexture.ReleaseTemporary(reflectRT);
    }

    // Update is called once per frame
    private void OnPreRender()
    {
        if (inited == false) return;
        cmr.farClipPlane = Camera.main.farClipPlane;
        cmr.nearClipPlane = Camera.main.nearClipPlane;
        cmr.fieldOfView = Camera.main.fieldOfView;
        cmr.orthographic = Camera.main.orthographic;
        transform.position = Camera.main.transform.position;
        transform.rotation = Camera.main.transform.rotation;
        
       //cmr.fieldOfView= Camera.main.fieldOfView;
 
    }

    public void useBillboardMode(bool useBDMode)
    {
       transform.parent.Find("realitems").gameObject.SetActive(!useBDMode);
       transform.parent.Find("billboards8").gameObject.SetActive(useBDMode);
    }
}
