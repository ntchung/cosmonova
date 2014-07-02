using UnityEngine;
using System.Collections;

public class StateMainMenu : GameState {
	
	public UILabel VersionLabel;
	
	void Awake()
	{
		UIEventListener.Get (FindChild ("ButtonPlay")).onClick += (obj) =>
		{
			StartCoroutine (OnPlayButtonClick ());
		};		
	}
	
	private IEnumerator OnPlayButtonClick()
	{
		yield return StartCoroutine(Utils.WaitForRealSeconds(0.25f));
		
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
