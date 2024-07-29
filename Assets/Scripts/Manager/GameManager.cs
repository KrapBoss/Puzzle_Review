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
/// 1. ���ÿ��� ���Ǵ� ���� ����
/// </summary>
public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public LocalGameState State { get; private set; }

    //�ش� ���·� ����� ���� ȣ��Ǿ�� �Ǵ� �̺�Ʈ
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
                Debug.Log("������ ���� ���·� ����");
                PlayEvent(state);
                break;
            case LocalGameState.Paying:
                Debug.Log("������ �������� ����.");
                PlayEvent(state);
                break;
            case LocalGameState.Pause:
                Debug.Log("������ �������·� ����");
                PlayEvent(state);
                break;
            case LocalGameState.End:
                Debug.Log("������ ���� ����");
                PlayEvent(state);
                break;
        }
        
        State = state;
    }

    //�� ���¿� ���� �̺�Ʈ �Լ��� �����ϴ� ��
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

    //������ �̺�Ʈ�� �����ŵ�ϴ�.
    void PlayEvent(LocalGameState state)
    {
        if (GameStateObservor.ContainsKey(state))
        {
            GameStateObservor[state].Invoke();
        }
        else
        {
            CustomDebug.PrintE($"{state} Ű�� �����ϴ�.");
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
