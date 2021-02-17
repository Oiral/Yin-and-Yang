using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EditorScreenCapture : MonoBehaviour
{
    public string filePath;

    [ContextMenu("Take Screen Shot")]
    public void ScreenCap()
    {
        ScreenCapture.CaptureScreenshot(Directory.GetCurrentDirectory() + filePath);
        Debug.Log(Directory.GetCurrentDirectory() + filePath);
    }
}
