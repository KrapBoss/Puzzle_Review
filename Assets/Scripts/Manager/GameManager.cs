using Custom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocalGameState
{
    None ,Starting, Paying, Pause, End
}

/// <summary>
/// 1. 로컬에서 사용되는 게임 정보
/// </summary>
public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public LocalGameState State { get; private set; }

    //해당 상태로 변경될 때에 호출되어야 되는 이벤트
    Dictionary<LocalGameState, Action> GameStateObservor = new Dictionary<LocalGameState, Action>();

    private void Awake()
    {
        if (instance == null) instance = this;

        State = LocalGameState.Starting;
    }

    public void GameStateChage(LocalGameState state)
    {
        if (State == state) return;

        
        switch (state)
        {
            case LocalGameState.Starting:
                Debug.Log("게임이 시작 상태로 변경");
                PlayEvent(state);
                break;
            case LocalGameState.Paying:
                Debug.Log("게임이 진행으로 변경.");
                PlayEvent(state);
                break;
            case LocalGameState.Pause:
                Debug.Log("게임이 정지상태로 변경");
                PlayEvent(state);
                break;
            case LocalGameState.End:
                Debug.Log("게임이 종료 상태");
                PlayEvent(state);
                break;
        }
        
        State = state;
    }

    //각 상태에 따른 이벤트 함수를 저장하는 것
    public void InsertGameStateAction(LocalGameState state, Action action)
    {
        if (!GameStateObservor.ContainsKey(state)) GameStateObservor[state] = action;
        GameStateObservor[state] += action;
    }

    public void DeleteGameStateAction(LocalGameState state, Action action)
    {
        if (GameStateObservor.ContainsKey(state))
        {
            GameStateObservor[state] -= action;
        }
    }

    //지정된 이벤트를 실행시킵니다.
    void PlayEvent(LocalGameState state)
    {
        if (GameStateObservor.ContainsKey(state))
        {
            GameStateObservor[state].Invoke();
        }
        else
        {
            CustomDebug.PrintE($"{state} 키가 없습니다.");
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
