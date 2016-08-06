using UnityEngine;
using System.Collections;

public class WebViewTouchManager : MonoBehaviour {
	
	public EasyWebViewCtrl [] m_srcWebView;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void LateUpdate()
	{
		ConverTouchData();
	}
	
	Vector2 m_vec2Prev = Vector2.zero;

	public void ConverTouchData()
	{
		if(m_srcWebView == null)
			return;
		
		if(m_srcWebView[0] == null)
			return;
		
		if(m_srcWebView[0].m_bCheckFBO == false)
			return;
		
		m_srcWebView[0].Call_StartConvertTouch();


		int iCount = m_srcWebView[0].Call_GetTouchCount();


		if(iCount != 0)
		{


			int [] iValueX =  m_srcWebView[0].Call_GetTouchValueX();
			int [] iValueY = m_srcWebView[0].Call_GetTouchValueY();
			
			if(iValueX == null || iValueY == null)
				return;
			
			if(iValueX.Length == 0 || iValueY.Length == 0)
				return;
			
			if(iValueX.Length != iCount || iValueY.Length != iCount)
				return;
			
			
			int [] iOutValueX = new int[iCount];
			int [] iOutValueY = new int[iCount];
			
			bool [] bUse  = new bool[iCount];
			
			foreach(EasyWebViewCtrl src in m_srcWebView)
			{
				for(int i = 0; i < iCount; i++)
				{
					if(src.m_objTargetTouch == null)
						break;
				
					Vector2 vec2Touch = new Vector2(iValueX[i],iValueY[i]);
					
		
					Collider col = src.m_objTargetTouch.transform.GetComponent<Collider>();
				
					if(col != null)
					{
						
						Ray vRay = Camera.main.ScreenPointToRay(new Vector3(vec2Touch.x,Screen.height - vec2Touch.y,src.m_objTargetTouch.transform.position.z));
						
						
						RaycastHit hit;
						if(col.Raycast(vRay,out hit,100.0f))
						{
							m_vec2Prev = new Vector2((int)(hit.textureCoord.x * (float)Screen.width),Screen.height - (int)(hit.textureCoord.y * (float)Screen.height));
							m_vec2Prev = new Vector2( m_vec2Prev.x * ((float)src.m_iWidth / (float)Screen.width),m_vec2Prev.y * ((float)src.m_iHeight / (float)Screen.height));
							//Debug.Log(m_vec2Prev);
							//Call_SetTouchValue(i,(int)m_vec2Prev.x,(int)m_vec2Prev.y);
							
							bUse[i] = true;
							
						}
						else
						{
							bUse[i] = false;
						}
						
						iOutValueX[i] = (int)m_vec2Prev.x;
						iOutValueY[i] = (int)m_vec2Prev.y;
						
					}
					
					
					
					
				}
				
				src.Call_SetTouchValue(iOutValueX,iOutValueY,bUse);
			}
			
			
		}
		

		m_srcWebView[0].Call_ClearTouchValue();
	}
	
	
}
