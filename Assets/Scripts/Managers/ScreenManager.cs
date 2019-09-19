using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Image bgScreen;
    public RawImage p1Screen, p2Screen, p3Screen, p4Screen;
    public Canvas canvas;
    float canvasWidth, canvasHeight;

    private void Start()
    {
        
        canvas = GetComponentInParent<Canvas>();
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        bgScreen.rectTransform.sizeDelta = new Vector2(canvasWidth, canvasHeight);
        bgScreen.rectTransform.position = new Vector2(0, canvasHeight);
        
    }


    public void UpdateScreenRects()
    {

        GameObject cameraList = GameManager.instance.cameraList;

        if(cameraList.transform.childCount == 1)
        {
            RenderTexture texture = new RenderTexture((int)canvasWidth, (int)canvasHeight, 16, RenderTextureFormat.ARGB32);
            p1Screen.texture = texture;
            cameraList.transform.GetChild(0).GetComponent<Camera>().targetTexture = texture;
            p1Screen.rectTransform.sizeDelta = new Vector2(canvasWidth, canvasHeight);
            p1Screen.rectTransform.position = new Vector2(0, canvasHeight);
        }
        else if (cameraList.transform.childCount == 2)
        {
            RenderTexture texture = new RenderTexture((int)canvasWidth, (int)canvasHeight, 16, RenderTextureFormat.ARGB32);
            p1Screen.texture = texture;
            cameraList.transform.GetChild(0).GetComponent<Camera>().targetTexture = texture;
            p1Screen.rectTransform.sizeDelta = new Vector2(canvasWidth, canvasHeight *.5f);

            RenderTexture texture2 = new RenderTexture((int)canvasWidth, (int)canvasHeight, 16, RenderTextureFormat.ARGB32);
            p2Screen.texture = texture2;
            cameraList.transform.GetChild(1).GetComponent<Camera>().targetTexture = texture2;
            p2Screen.rectTransform.position = new Vector2(0, canvasHeight * 0.5f);
            p2Screen.rectTransform.sizeDelta = new Vector2(canvasWidth, canvasHeight *.5f);

        }else if (cameraList.transform.childCount == 3)
        {
            p1Screen.rectTransform.sizeDelta = new Vector2(canvasWidth * .5f, canvasHeight * .5f);
            p2Screen.rectTransform.sizeDelta = new Vector2(canvasWidth * .5f, canvasHeight * .5f);
            p2Screen.rectTransform.position = new Vector2(canvasWidth *.25f, canvasHeight);

            p3Screen.rectTransform.position = new Vector2(0, canvasHeight * 0.25f);
            p3Screen.rectTransform.sizeDelta = new Vector2(canvasWidth, canvasHeight * .5f);
        }

        //look for images
        //split screen into images at right ratio
    }

}
