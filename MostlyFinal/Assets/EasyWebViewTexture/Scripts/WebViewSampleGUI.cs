using UnityEngine;
using System.Collections;

public class WebViewSampleGUI : MonoBehaviour {
	
	public EasyWebViewCtrl scrMedia;
	public GameObject m_objTouch1;
	public GameObject m_objTouch2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		
	
		if( GUI.Button(new Rect(50,50,100,100),"Load naver"))
		{
			scrMedia.Load("http://www.naver.com");
		}

		if( GUI.Button(new Rect(50,250,100,100),"Load google"))
		{
			scrMedia.Load("http://www.google.com");
		}

		if( GUI.Button(new Rect(50,450,100,100),"Load html5test"))
		{
			scrMedia.Load("http://beta.html5test.com/");
		}


		if( GUI.Button(new Rect(50,550,100,100),"Touch Cube"))
		{
			scrMedia.m_objTargetTouch = m_objTouch1;
		}

		if( GUI.Button(new Rect(150,550,100,100),"Touch Quad"))
		{
			scrMedia.m_objTargetTouch = m_objTouch2;
		}
	

		if( GUI.Button(new Rect(250,50,100,100),"1024*1024"))
		{
			scrMedia.SetWebViewSize(1024,1024);
		}

		if( GUI.Button(new Rect(450,50,100,100),"1920*1080"))
		{
			scrMedia.SetWebViewSize(1080,1920);
		}
		
		if( GUI.Button(new Rect(250,250,100,100),"GoBack"))
		{
			scrMedia.GoBack();
		}

		if( GUI.Button(new Rect(450,250,100,100),"GoForward"))
		{
			scrMedia.GoForward();
		}
		
		if( GUI.Button(new Rect(450,450,100,100),"Load:" + scrMedia.GetProgress()))
		{
			
		}
		
	
		
	}
	
	
}
