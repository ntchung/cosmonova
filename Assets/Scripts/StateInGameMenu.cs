using UnityEngine;
using System.Collections;

public class StateInGameMenu : GameState {
	
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
	}
	
	public override void OnExit()
	{
	}
	
	public override void OnBackKey()
	{
		StateManager.Instance.PushState (StateManager.Instance.PauseMenu);		
	}
}
