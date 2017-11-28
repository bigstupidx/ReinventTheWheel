// Saves screenshot as PNG file.
using UnityEngine;
using System.Collections;
using System.IO;

public class PNGUploader : MonoBehaviour
{
	Color color;
	DrawController drawer;

	void Start()
	{
		drawer = GameObject.Find ("Draw Controller").GetComponent<DrawController> ();
		color = drawer.color;
	}

	public void OnClick()
	{
		Debug.Log (Application.dataPath + "/");
		StartCoroutine(DoStuff ());
	}
	// Take a shot immediately
	public IEnumerator DoStuff()
	{
		yield return UploadPNG();
	}

	public IEnumerator UploadPNG()
	{
		// We should only read the screen buffer after rendering is complete
		yield return new WaitForEndOfFrame();

		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);

		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);

		//iterate through the pixels and see if the pixel coordinates are within the shape that we are carving out, 
		//make them transparent if they meet the condition

		for (int x = 0; x < tex.width; x++)
		{
			for (int y = 0; y < tex.height; y++)
			{
				//get the color of the pixel at the current coordinates
				Color colorOfPixel = tex.GetPixel(x, y);

				//if the pixel is already transparent, go to the next iteration
				if (colorOfPixel.a == 0) {
					continue;
				}

				if (colorOfPixel != color) {
					colorOfPixel.a = 0;
					tex.SetPixel(x, y, colorOfPixel);
				}

			}
		}

		tex.Apply();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Object.Destroy(tex);

		// For testing purposes, also write to a file in the project folder
		File.WriteAllBytes(Application.dataPath + "/b.png", bytes);


		// Create a Web Form
		WWWForm form = new WWWForm();
		form.AddField("frameCount", Time.frameCount.ToString());
		form.AddBinaryData("fileUpload", bytes);

		// Upload to a cgi script
		WWW w = new WWW("http://localhost/cgi-bin/env.cgi?post", form);
		yield return w;

		if (w.error != null)
		{
			Debug.Log(w.error);
		}
		else
		{
			Debug.Log("Finished Uploading Screenshot");
		}

	}
}