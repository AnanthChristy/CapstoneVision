using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Collections;

public class EasyWebViewCtrl : MonoBehaviour {
	
	public string m_strUrl;
	public GameObject [] m_TargetMaterial; 
	public GameObject m_objTargetTouch;
	private Texture2D m_VideoTexture = null;

	public int m_iWidth = 720;
	public int m_iHeight = 1280;



	
	

	public enum MEDIA_SCALE
	{
		SCALE_X_TO_Y	= 0,
		SCALE_X_TO_Z	= 1,
		SCALE_Y_TO_X	= 2,
		SCALE_Y_TO_Z	= 3,
		SCALE_Z_TO_X	= 4,
		SCALE_Z_TO_Y	= 5,
	}
	
	bool m_bFirst = false;
	
	public MEDIA_SCALE m_ScaleValue;
	public GameObject m_objResize = null;

	
	void Awake(){
		

		m_VideoTexture = new Texture2D(0,0,TextureFormat.RGB565,false);
		m_VideoTexture.filterMode = FilterMode.Bilinear;
	    m_VideoTexture.wrapMode = TextureWrapMode.Clamp;

		



	}
	// Use this for initialization
	void Start () {
		Call_SetUnityActivity();
		Call_SetScreenSize(Screen.width, Screen.height);
		Call_SetUnityTexture(m_VideoTexture.GetNativeTextureID());
		Call_SetWindowSize(m_iWidth, m_iHeight );
		
	}
	
	
	public bool m_bCheckFBO = false;
	
	
	
	
	void Update()
	{
		
		if(m_bFirst == false)
		{
		
			string strUrl = m_strUrl.Trim();

			if(Call_Load(strUrl) == false)
				return;
	
			m_bFirst = true;
			
		
		}
	
		if(m_bFirst == true)
		{
			
			if(m_bCheckFBO == false)
			{
				
				m_bCheckFBO = true;
				
				if(m_objResize != null)
				{
					int iWidth = m_iWidth;
					int iHeight = m_iHeight ;
					
					float fRatio = (float)iHeight / (float)iWidth;
					
					if( m_ScaleValue == MEDIA_SCALE.SCALE_X_TO_Y)
					{
						m_objResize.transform.localScale 
						= new Vector3(m_objResize.transform.localScale.x
								,m_objResize.transform.localScale.x * fRatio
								,m_objResize.transform.localScale.z);
					}
					else if( m_ScaleValue == MEDIA_SCALE.SCALE_X_TO_Z)
					{
						m_objResize.transform.localScale 
						= new Vector3(m_objResize.transform.localScale.x
								,m_objResize.transform.localScale.y
								,m_objResize.transform.localScale.x * fRatio);
					}
					else if( m_ScaleValue == MEDIA_SCALE.SCALE_Y_TO_X)
					{
						m_objResize.transform.localScale 
						= new Vector3(m_objResize.transform.localScale.y * fRatio
								,m_objResize.transform.localScale.y
								,m_objResize.transform.localScale.z);
					}
					else if( m_ScaleValue == MEDIA_SCALE.SCALE_Y_TO_Z)
					{
						m_objResize.transform.localScale 
						= new Vector3(m_objResize.transform.localScale.x
								,m_objResize.transform.localScale.y
								,m_objResize.transform.localScale.y * fRatio);
					}
					else if( m_ScaleValue == MEDIA_SCALE.SCALE_Z_TO_X)
					{
						m_objResize.transform.localScale 
						= new Vector3(m_objResize.transform.localScale.z * fRatio
								,m_objResize.transform.localScale.y
								,m_objResize.transform.localScale.z);
					}
					else if( m_ScaleValue == MEDIA_SCALE.SCALE_Z_TO_Y)
					{
						m_objResize.transform.localScale 
						= new Vector3(m_objResize.transform.localScale.x
								,m_objResize.transform.localScale.z * fRatio
								,m_objResize.transform.localScale.z);
					}
					else 
					{
						m_objResize.transform.localScale 
						= new Vector3(m_objResize.transform.localScale.x,m_objResize.transform.localScale.y,m_objResize.transform.localScale.z);
					}
					
				}
			}
			
		
			
			Call_UpdateVideoTexture();
		}
	
		
	
		

	}


	
	


	public void SetWebViewSize(int iWidth, int iHeight)
	{
		m_iWidth = iWidth;
		m_iHeight = iHeight;

		m_bFirst = false;
		m_bCheckFBO = false;

		Call_SetWindowSize(iWidth, iHeight );
	}
	
	
	void OnDestroy()
	{
		Call_UnLoad();
		
		if(m_VideoTexture != null)
			Destroy(m_VideoTexture);
		
		Call_Destroy();
	}
	
	void OnApplicationPause(bool bPause)
	{
		
	}
	
	public Texture2D GetVideoTexture()
	{
		return m_VideoTexture;
	}
	

	
	public void Load(string strUrl)
	{
		m_strUrl = strUrl;
		SetWebViewSize(m_iWidth, m_iHeight);
	
		
	}
	
	public void UnLoad()
	{
		m_bCheckFBO = false;
		
		Call_UnLoad();

	}
	
	public void GoBack()
	{
		Call_GoBack();
	}
	
	public void GoForward()
	{
		Call_GoForward();
	}
	
	public int GetProgress()
	{
		return Call_GetProgress();
	}
	
	
	
	
	


#if !UNITY_EDITOR

#if UNITY_ANDROID

    private AndroidJavaObject javaObj = null;
	
	

    private AndroidJavaObject GetJavaObject()
    {
        if (javaObj == null)
        {
			
			javaObj = new AndroidJavaObject("com.easywebviewtexture.EasyWebviewTexture");
        	
        }								

        return javaObj;
    }

   

  
	private void Call_Destroy()
	{
		GetJavaObject().Call("Destroy");
	}
	
	
	private void Call_UnLoad()
	{
		GetJavaObject().Call("UnLoad");
	}
	
	private bool Call_Load(string strFileName)
	{
		return GetJavaObject().Call<bool>("Load", strFileName);
	}
	
	private void Call_UpdateVideoTexture()
	{
		if(m_TargetMaterial != null)
		{
			foreach(GameObject obj in m_TargetMaterial)
			{
				if(obj==null)
					continue;

				if(obj.GetComponent<MeshRenderer>().material.mainTexture != m_VideoTexture)
				{
					obj.GetComponent<MeshRenderer>().material.mainTexture = m_VideoTexture;
				}
			}

			
		}
		
		GetJavaObject().Call("UpdateVideoTexture");
	}
	

	private void Call_SetUnityTexture(int iTextureID)
	{
		GetJavaObject().Call("SetUnityTexture",iTextureID);
	}


	private void Call_SetScreenSize(int iWidth, int iHeight)
	{
		GetJavaObject().Call("SetScreenSize",iWidth, iHeight);
	}

	private void Call_SetWindowSize(int iWidth, int iHeight)
	{
		GetJavaObject().Call("SetWindowSize",iWidth, iHeight);
	}
	

	 private void Call_SetUnityActivity()
    {
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        GetJavaObject().Call("SetUnityActivity",jo);
    }


	public int Call_GetTouchCount()
	{
		return GetJavaObject().Call<int>("GetTouchCount");
	}

	public int[] Call_GetTouchValueX()
	{
		return GetJavaObject().Call<int []>("GetTouchValueX") ;
	}

	public int[] Call_GetTouchValueY()
	{
		return  GetJavaObject().Call<int []>("GetTouchValueY") ;
	}



	public void Call_SetTouchValue(  int []iX, int []iY,bool [] bUse)
	{
	
		GetJavaObject().Call("SetTouchValue", iX, iY,bUse);
	}

	public void Call_ClearTouchValue()
	{
		GetJavaObject().Call("ClearTouchValue");
	}

	public void Call_StartConvertTouch()
	{
		GetJavaObject().Call("StartConvertTouch");
	}
	
	public void Call_GoBack()
	{
		GetJavaObject().Call("GoBack");
	}
	
	public void Call_GoForward()
	{
		GetJavaObject().Call("GoForward");
	}

	public int Call_GetProgress()
	{
		return GetJavaObject().Call<int>("GetProgress");
	}

	

    
#endif
	
#else // !UNITY_EDITOR

  
	
	private void Call_Destroy()
	{

	}
	
	private void Call_UnLoad()
	{

	}
	
	private bool Call_Load(string strFileName)
	{
		return false;
	}
	
	private void Call_UpdateVideoTexture()
	{

	}

	private void Call_SetUnityTexture(int iTextureID)
	{
	
	}

	private void Call_SetScreenSize(int iWidth, int iHeight)
	{
	
	}
	
	private void Call_SetWindowSize(int iWidth, int iHeight)
	{
		
	}

	
	public void Call_SetUnityActivity()
    {
        
    }
	

	public int Call_GetTouchCount()
	{
		return 0;
	}
	
	public int[] Call_GetTouchValueX()
	{
		return null;
	}

	public int[] Call_GetTouchValueY()
	{
		return null;
	}
	


	public void Call_SetTouchValue(  int []iX, int []iY,bool [] bUse)
	{

	}
	
	public void Call_ClearTouchValue()
	{

	}
	
	public void Call_StartConvertTouch()
	{

	}
	
	public void Call_GoBack()
	{
		
	}
	
	public void Call_GoForward()
	{
		
	}
	
	public int Call_GetProgress()
	{
		return 0;
	}



	


#endif // !UNITY_EDITOR
	

}
