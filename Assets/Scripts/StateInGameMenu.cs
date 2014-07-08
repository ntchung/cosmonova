using UnityEngine;
using System.Collections;

public class StateInGameMenu : GameState {
	
	private int screenshotCount = 0;
	
	void Awake()
	{
	}
	
	public void OnPauseButtonClick()
	{
		StateManager.Instance.PushState (StateManager.Instance.PauseMenu);				
	}
	
	public override void OnEnter()
	{
	}
	
	public override void OnUpdate()
	{
		if( Input.GetKeyUp(KeyCode.Q) )
		{
			Application.CaptureScreenshot("screenshot" + screenshotCount + ".png");
			++screenshotCount;
		}
		
		if( Input.GetKeyUp(KeyCode.P) )
		{
			this.gameObject.SetActive(false);
		}
	}
	
	public override void OnExit()
	{
	}
	
	public override void OnBackKey()
	{
		StateManager.Instance.PushState (StateManager.Instance.PauseMenu);		
		Application.Quit();
	}
}
