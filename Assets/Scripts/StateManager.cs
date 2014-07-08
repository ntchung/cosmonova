﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {

	private static StateManager g_instance = null;

	public GameState MainMenu;
	public GameState InGameMenu;
	public GameState Leaderboard;
	public GameState Shop;
	public GameState GameOver;
	public GameState PauseMenu;
	public GameState Help;
	public GameState About;

	private GameState m_currentState;
	private GameState m_nextState;
	private GameState m_pendState;

	private Stack<GameState> m_stackStates = new Stack<GameState>();

	public static StateManager Instance
	{
		get { return g_instance; }
	}

	void Awake()
	{
		Global.Init ();

		MainMenu.gameObject.SetActive (false);
		/*InGameMenu.gameObject.SetActive (false);
		Leaderboard.gameObject.SetActive (false);
		Shop.gameObject.SetActive (false);
		GameOver.gameObject.SetActive (false);
		PauseMenu.gameObject.SetActive (false);
		Help.gameObject.SetActive(false);*/

		g_instance = this;
	}

	void Start()
	{			
		m_currentState = InGameMenu;
		m_currentState.gameObject.SetActive (true);
		m_currentState.OnEnter ();

		m_nextState = m_currentState;
		m_pendState = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_currentState != m_nextState) {
			m_currentState.OnExit();
			m_currentState.gameObject.SetActive(false);

			if( m_pendState != null )
			{
				m_stackStates.Push (m_nextState);			
				m_nextState = m_pendState;
				m_pendState = null;
			}
			
			m_currentState = m_nextState;
			m_currentState.gameObject.SetActive(true);
			m_currentState.OnEnter();			
		}

		m_currentState.OnUpdate ();
		
		if( Input.GetKeyDown(KeyCode.Escape) )
		{
			m_currentState.OnBackKey();			
		}
	}
	
	void LateUpdate()
	{
		Global.audioCount = 0;
	}

	public void PushState(GameState state)
	{
		m_stackStates.Push (m_currentState);
		m_nextState = state;
	}

	public void PopState()
	{
		m_nextState = m_stackStates.Pop ();
	}

	public void SetState(GameState state)
	{
		m_stackStates.Clear ();
		m_nextState = state;
	}
	
	public void PendState(GameState state)
	{
		m_pendState = state;
	}
	
	public bool IsInMainMenu
	{
		get { return m_currentState == MainMenu; }
	}
	
	public bool IsInLeaderboard
	{
		get { return m_currentState == Leaderboard; }
	}
	
	public bool IsInShop
	{
		get { return m_currentState == Shop; }
	}
	
	public bool IsInHelp
	{
		get { return m_currentState == Help; }
	}
}
