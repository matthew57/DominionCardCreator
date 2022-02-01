using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShotter : MonoBehaviour
{

    public Camera RenderCam;


    // Update is called once per frame
    void Update()
    {/*
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenShot();
        }

        if (Input.GetKey(KeyCode.T))
            { RTImage(); }*/
    }

    public void TakeScreenShot()
    {
 
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/fileName.png" ,3);
        Debug.Log(Application.dataPath + "/fileName.png");
        




    }

    public Texture2D RTImage(string exp, string fileName, bool vertical)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = RenderCam.targetTexture;

        // Render the camera's view.
        RenderCam.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(RenderCam.targetTexture.width, RenderCam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, RenderCam.targetTexture.width, RenderCam.targetTexture.height), 0, 0);
        image.Apply();
        Debug.Log(RenderCam.targetTexture.width + "  " + RenderCam.targetTexture.height);

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;

        var bytes = image.EncodeToPNG();
        Destroy(image);

        //Writing bytes to a file

        if (vertical)
        {
            Directory.CreateDirectory(Application.dataPath + "/" + exp + "/Vert");
            File.WriteAllBytes(Application.dataPath + "/" + exp + "/Vert/" + fileName + ".png", bytes);
        }
        else
        {
            Directory.CreateDirectory(Application.dataPath + "/" + exp);
            File.WriteAllBytes(Application.dataPath + "/" + exp + "/" + fileName + ".png", bytes);
        }
        Debug.Log("Written");
        return image;
    }

}
