using UnityEngine;
using System.Collections;

public class StateInGameMenu : GameState {
	
	void Awake()
	{
		GameObject pauseButton = FindChild ("ButtonPause");

		if (pauseButton != null)
		{
			UIEventListener.Get(pauseButton).onClick += (obj) =>
			{
				StartCoroutine (OnPauseButtonClick ());
			};		
		}
	}
	
	private IEnumerator OnPauseButtonClick()
	{
		yield return StartCoroutine(Utils.WaitForRealSeconds(0.25f));
		
		StateManager.Instance.PushState (StateManager.Instance.PauseMenu);				
	}
	
	public override void OnEnter()
	{
	}
	
	public override void OnUpdate()
	{
	}
	
	public override void OnExit()
	{
	}
	
	public override void OnBackKey()
	{
		StateManager.Instance.PushState (StateManager.Instance.PauseMenu);		
	}
}
