using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ModelVariantGroup : MonoBehaviour
{
	public Renderer[] renderersList;
	string[] renderersNameList;
	
	public int activeRendererIndex = 0;
	
	void Awake()
	{
		ApplyRenderersChange();
	}
	
	void OnEnable()
	{
		ApplyRenderersChange();
	}

	void OnDisable()
	{
		RestoreRenderersHideFlags();
	}

	public void UpdateRendererList()
	{
		renderersList = GetComponentsInChildren<Renderer>();
		if (activeRendererIndex >= renderersList.Length) activeRendererIndex = 0;
		renderersNameList = new string[renderersList.Length];
		
		for (int i = 0; i < renderersList.Length; i++)
		{
			renderersNameList[i] = renderersList[i].gameObject.name;
		}
	}
	
	public string[] GetRendererNames() 
	{
		return renderersNameList;
	}

	public void ApplyRenderersChange()
	{
		if(renderersList == null)
		{
			return;
		}
		for (int i = 0; i < renderersList.Length; i++)
		{
			if(renderersList[i] == null)
			{
				continue;
			}
			renderersList[i].enabled = false;
			if(renderersList[i].transform.GetComponent<Collider>()){
				renderersList[i].transform.GetComponent<Collider>().enabled = false;
			}
			
			renderersList[i].gameObject.hideFlags = HideFlags.HideInHierarchy;
		}	

		if(renderersList[activeRendererIndex] != null)
		{
			renderersList[activeRendererIndex].enabled = true;	
			if(renderersList[activeRendererIndex].transform.GetComponent<Collider>()){
				renderersList[activeRendererIndex].transform.GetComponent<Collider>().enabled = true;
				
			}
			renderersList[activeRendererIndex].gameObject.hideFlags = HideFlags.None;		
		}
		
	}

	public void RestoreRenderersHideFlags()
	{
		
		for (int i = 0; i < renderersList.Length; i++)
		{
			if(renderersList[i] == null)
			{
				continue;
			}
			renderersList[i].gameObject.hideFlags = HideFlags.None;
		}		
	}
}
