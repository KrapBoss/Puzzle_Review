using Custom;
using Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Network
{
    //��Ʈ��ũ ���� ������ ��ü���� ���� ����
    public class NetworkGameController : NetworkBehaviour
    {
        enum GameState // �� ���� ����
        {
            Starting, Running, Ending
        }

        public static NetworkGameController Instance { get; private set; }

        List<PlayerDataNetwork> _players = new List<PlayerDataNetwork>();

        [Networked]
        NetworkString<_32> PuzzleName { get; set; } //������ ���� �̸�

        [Networked]
        GameState State { get; set; }   //���� ������ ���� �Ǵ�

        [Networked]
        TickTimer tickTimer { get; set; }   // ��Ʈ��ũ �ð� ����

        TimerUI timerUI;
        [Networked] bool delay { get; set; } // ���� �� ������ ������ �˸�

        //�⺻ ��Ʈ��ũ ������ ���
        string PrefabPath = "Network/Puzzle";

        private void Awake()
        {
            CustomDebug.PrintW("NetworkGameController ����");

            Instance = this;

            //�ش� ������Ʈ�� ������ ������ Ŭ���̾�Ʈ���� ��Ÿ��.
            GetComponent<NetworkObject>().Flags |= NetworkObjectFlags.MasterClientObject;
        }

        private void OnDestroy()
        {
            if (Instance == this) { Instance = null; }
            else { throw new InvalidOperationException(); }
        }

        public override void Spawned()
        {
            base.Spawned();

            //������ Ŭ���̾�Ʈ�� ���� ���� ���� ����
            if (HasStateAuthority)
            {
                CustomDebug.PrintW("������ ������ �����մϴ�.");

                State = GameState.Starting;

                PuzzleName = GetRandomPuzzleName();
                Resources.UnloadUnusedAssets();
                CustomDebug.PrintW($"{PuzzleName}");

                //�÷��̾� ����
                FindObjectOfType<PlayerChracterSpawner>().ReadyGame();

                tickTimer = TickTimer.CreateFromSeconds(Runner, 60 * 3);
            }

            timerUI = FindObjectOfType<TimerUI>();

            TopCanvasInMulti.Instance.Blind.gameObject.SetActive(true);

            FindObjectOfType<TitleUI>().SetTitle($"��{PuzzleName}��");
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            CustomDebug.PrintE("�������� ���� ���� ����");
            ShutDown();
        }

        //��Ʈ��ũ���� �ֱ������� �׷����� ���
        public override void Render()
        {
            //�� �ܰ迡 ���� ���� ���� ���� ������Ʈ
            switch (State)
            {
                case GameState.Starting:
                    StartingGame();
                    break;
                case GameState.Running:
                    RunningGame();
                    break;
                case GameState.Ending:
                    EndingGame();
                    break;

            }
        }

        void StartingGame()
        {

            //-----------------Master----------------
            if (HasStateAuthority)
            {
                //���� ���� ��� �ð� �ο�
                if (_players.Count >= 2 && !delay)
                {
                    tickTimer = TickTimer.CreateFromSeconds(Runner, 3);
                    delay = true;
                }

                //������ ���� �� ������ �����մϴ�.
                if (tickTimer.Expired(Runner))
                {
                    //���� �ܰ�� ����
                    State = GameState.Running;
                    tickTimer = TickTimer.CreateFromSeconds(Runner, 60 * 4);
                }
            }


            //-----------------------Local---------------

            CustomDebug.PrintE("��Ʈ��ũ ���� Starting.");

            float remain = tickTimer.RemainingTime(Runner) ?? 0;
            timerUI.SetTime(remain);

            if (delay)
            {
                TopCanvasInMulti.Instance.Blind.SetText($"Ready! {remain: 0.#}");

                //���� �� �ʱ�ȭ ����
                if(remain <= 1.0f)
                {
                    //���ÿ��� �� ���¿� ���� ���ǵ� 
                    GameManager.Instance.GameStateChage(LocalGameState.Paying);
                }
            }
        }

        void RunningGame()
        {
            CustomDebug.PrintE("��Ʈ��ũ ���� Running.");

            //-----------------------Local---------------

            TopCanvasInMulti.Instance.Blind.gameObject.SetActive(false);

            float remain = tickTimer.RemainingTime(Runner) ?? 0;
            timerUI.SetTime(remain);


            //-----------------Master----------------

            if (HasStateAuthority)
            {
                //���� ���� ���� �Ǵ�
                CheckEnd();
            }
        }

        bool CheckEnd()
        {
            if (GameHasEnd())
            {
                tickTimer = TickTimer.CreateFromSeconds(Runner, 15.0f);
                return true;
            }

            return false;
        }

        bool GameHasEnd()
        {
            //-----------------------Local---------------

            //���� 1 : �ð� �ʰ�
            if (tickTimer.Expired(Runner))
            {
                State = GameState.Ending;
                return true;
            }

            //���� 2 : ������ �ϼ�
            foreach (var player in _players)
            {
                if (player.Perfection == 1.0f)
                {
                    State = GameState.Ending;
                    return true;
                }
            }

            //���� 3 : �÷��̾��� ����
            if (_players.Count < 2)
            {
                State = GameState.Ending;
                return true;
            }

            return false;
        }

        void EndingGame()
        {
            CustomDebug.PrintE("��Ʈ��ũ ������ �����մϴ�.");
            TopCanvasInMulti.Instance.Result.ShowResult(_players.ToArray());
            TopCanvasInMulti.Instance.Blind.gameObject.SetActive(false);

            //���� �ð� ���� �� ���� ����
            if (tickTimer.Expired(Runner))
            {
                ShutDown();
            }
        }

        

        public void RegistPlayer(PlayerDataNetwork player)
        {
            //������ ����
            if (State == GameState.Ending) return;

            if (!_players.Contains(player)) _players.Add(player);
            CustomDebug.Print("�÷��̾ �߰��Ǿ����ϴ�.");
        }
        public void RemovePlayer(PlayerDataNetwork player)
        {
            if (_players.Contains(player)) _players.Remove(player);
            CustomDebug.Print("�÷��̾ ���ŵǾ����ϴ�.");
        }

        //���� ��Ʈ��ũ ���� �� ������ �Ѱ��� �����ɴϴ�.
        string GetRandomPuzzleName()
        {
            // Resources ���� ���� Ư�� ��ο��� ��� GameObject�� �ε�
            var objects = Resources.LoadAll<GameObject>(PrefabPath);
            //foreach (GameObject go in objects) CustomDebug.PrintW(go.name);

            // �� GameObject�� �̸��� �迭�� ��ȯ
            string[] puzzleNames = objects.Select(obj => obj.name).ToArray();
            foreach (string go in puzzleNames) CustomDebug.PrintW(go);

            if (puzzleNames.Length < 1) throw new Exception("���ҽ����� ��Ʈ��ũ ������Ʈ�� ã�� �� �����ϴ�.");

            return puzzleNames[Random.Range(0, puzzleNames.Length)];
        }

        public string GetPuzzlePath()
        {
            return $"{PrefabPath}/{PuzzleName}";
        }

        //���� ����
        void ShutDown()
        {
            CustomDebug.PrintE("NetworkGAmeController :: ShutDown");
            if (Runner.IsRunning)
            {
                Runner.Shutdown();
            }
        }
    }
}
