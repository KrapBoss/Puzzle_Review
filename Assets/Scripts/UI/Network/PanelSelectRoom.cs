using Custom;
using Fusion;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    /// <summary>
    /// ���õ� �뿡 ���� ������ ����մϴ�.
    /// ��Ʈ��ũ�� ���� ���� ������ ��ݵ˴ϴ�.
    /// </summary>
    public class PanelSelectRoom : MonoBehaviour
    {
        //��Ʈ��ũ ������ ���� ������ ��ü
        [SerializeField] private NetworkRunner _networkRunnerPrefab = null;

        //�г���
        [SerializeField] private TextMeshProUGUI _nickName = null;

        // The Placeholder Text is not accessible through the TMP_InputField component so need a direct reference
        [SerializeField] private TextMeshProUGUI _nickNamePlaceholder = null;

        [SerializeField] private TMP_InputField _roomName = null;

        //���� �� ������ ���� ���
        [SerializeField] private string _gameScenePath = null;

        //������ ������ ��Ʈ��ũ ����
        private NetworkRunner _runnerInstance = null;

        // ���ο� ���� ������ ������ �õ��Ѵ�.
        public void StartShared()
        {
            SetPlayerData();
            StartGame(GameMode.Shared, _roomName.text, _gameScenePath);
        }

        //�÷��̾� ������ ����
        private void SetPlayerData()
        {
            //���� �г��� �ʵ尡 ����ִٸ�, PlaceHolder�� �ִ� �г����� �����ȴ�.
            if (string.IsNullOrWhiteSpace(_nickName.text))
            {
                PlayerLocalData.NickName = _nickNamePlaceholder.text;
            }
            else
            {
                PlayerLocalData.NickName = _nickName.text;
            }
        }

        //async ������ �׻� Promise�� ��ȯ�Ѵ�. // ��� ��� ���� ��
        private async void StartGame(GameMode mode, string roomName, string sceneName)
        {
            _runnerInstance = FindObjectOfType<NetworkRunner>();
            if (_runnerInstance == null) // �ڵ����� Dontdestroy�� �����ȴ�.
            {
                _runnerInstance = Instantiate(_networkRunnerPrefab);
            }

            // ǻ�� ���ʿ��� ����� �Է��� ������ ������ �˸�
            _runnerInstance.ProvideInput = true;

            StartGameArgs startGameArgs = new StartGameArgs()
            {
                //���� ���� ���
                GameMode = mode,

                //�� �̸����� ã��
                SessionName = roomName,

                PlayerCount = 2,

                //��ηκ��� �� �ε����� ������ Ȱ��ȭ�ϱ�
                Scene = SceneRef.FromIndex(SceneUtility.GetBuildIndexByScenePath(_gameScenePath)),

                //���� ���� �� ��Ʈ��ũ ������Ʈ Ǯ ���
                //ObjectProvider = _runnerInstance.GetComponent<NetworkObjectPoolDefault>(),
            };

            // GameMode.Host = Start a session with a specific name
            // GameMode.Client = Join a session with a specific name

            //���� ������ ���� ������ �����Ѵ�.
            //�뿡 ����
            var result = await _runnerInstance.StartGame(startGameArgs);

            CustomDebug.Print($"_runnerInstance.IsServer :: {_runnerInstance.IsServer}");

            ////����[Ŭ����]�� ���
            //if (_runnerInstance.IsServer)
            //{
            //    CustomDebug.Print("���� ���� ���� ���� �õ�");

            //    //�������� ���� ȣ���ϵ��� �մϴ�.
            //    await _runnerInstance.LoadScene(sceneName);
            //}
        }

        private void OnEnable()
        {
            // _nickNamePlaceholder.text = PlayerLocalData.NickName;
        }
        private void OnDisable()
        {
            _roomName.text = "";
        }
    }
}