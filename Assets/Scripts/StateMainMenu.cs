using UnityEngine;
using System.Collections;

public class StateMainMenu : GameState {
	
	public UILabel VersionLabel;
	
	void Awake()
	{
	}
	
	public void OnPlayButtonClick()
	{		
		StateManager.Instance.PushState (StateManager.Instance.InGameMenu);				
	}
	
	public override void OnEnter()
	{
		VersionLabel.text = "Version " + CurrentBundleVersion.version;
	}

	public override void OnUpdate()
	{
	}

	public override void OnExit()
	{
	}

	public override void OnBackKey()
	{
		Application.Quit();
	}
}
