using Fusion;
using Network;
using UnityEngine;
using System;
using Custom;

/// <summary>
/// ��Ʈ��ũ�� �����Ǵ� �÷��̾� ������ ������ �ֽ��ϴ�.
/// </summary>
public class PlayerDataNetwork : NetworkBehaviour
{
    [Networked]
    public NetworkString<_16>NickName { get; private set; }

    [Networked]
    public float Perfection { get; private set; }

    [Networked]
    public int Combo { get; private set; }

    [Networked]
    public string PlayerIcon {  get; private set; }

    //ĳ���� ������
    public SpriteRenderer spriteRenderer;

    NetworkGameController _NGC;

    StatePanel _statePanel;

    public override void Spawned()
    {
        //�ش� ĳ���Ϳ� ���� ���� �Ǵ�
        if (HasStateAuthority)
        {
            _NGC = NetworkGameController.Instance;

            spriteRenderer.enabled = false;

            CustomDebug.PrintE("�÷��̾� ĳ���� ���������� ���� ���� ����");
            NickName = PlayerLocalData.NickName;
            Perfection = 0;
            Combo = 0;

            GameObject puzzle = Resources.Load<GameObject>(_NGC.GetPuzzlePath());
            CustomDebug.PrintW($"{_NGC.GetPuzzlePath()}");
            if (puzzle == null) 
                throw new Exception("�÷��̾� ������ : ���� �ҷ����� ����");

            var _spawnedPuzzle=  Runner.Spawn(puzzle);

            PuzzleSet(_spawnedPuzzle);

            if (_spawnedPuzzle == null) throw new Exception("�÷��̾� ������ : ���� ���� ����");
        }


        //All Client
        CustomDebug.PrintE($"�÷��̾� ���� {NickName}");

        //��� Ŭ���̾�Ʈ���� ���� �� ���¹� ������Ʈ�� ���� ���
        _statePanel = FindObjectOfType<StatePanel>();
        _statePanel.RegisterPlayer(this);

        NetworkGameController.Instance.RegistPlayer(this);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        NetworkGameController.Instance.RemovePlayer(this);
        FindObjectOfType<StatePanel>()?.RemovePlayer(this);
    }


    //2���� �÷��̾ �����ϱ� ������ ���Ϸ� ��ġ��Ų��.
     Vector2 GetPuzzleSpawnPoint(PuzzleContainer container, int _index)
    {
        Vector2 point = Vector2.zero;
        int multiple = (_index == 0) ? -1 : 1;
        point.y += container.BackGroundSprite.bounds.size.y * multiple;
        point.y += container.BackGroundSprite.bounds.size.y*0.2f* multiple; //������
        return point;
    }

    //���� ��ġ ����
    void PuzzleSet(NetworkObject puzzle)
    {
        //���� �ʱ�ȭ
        PuzzleContainer container = puzzle.GetComponent<PuzzleContainer>();
        container.Initialize();

        //�̺�Ʈ ���
        container.Action_Fit += ChangeData;

        //�ڽ��� ������ ������ ����Ѵ�.
        FindObjectOfType<CameraOption>().Container = container;

        //���� ���� ��Ʈ�ѷ��� ���� ������ �����Ѵ�.
        int index = Runner.LocalPlayer.PlayerId % 2;    //��ġ�� ��� ���� ��
        //CustomDebug.PrintE($"�÷��̾� ��Ʈ��ũ ��ȣ {Runner.LocalPlayer.PlayerId}");

        container.transform.position = GetPuzzleSpawnPoint(container, index);
        //CustomDebug.PrintE($"{GetPuzzleSpawnPoint(container, index)}");
    }

    //������ ���� �� �г� ������Ʈ�� ���� �۾�
    public void ChangeData(float perfection)
    {
        RpcChangeData(perfection);
    }

    //�ش� ������Ʈ�� ���� ������ ���� �÷��̾ ȣ���ϰ�, ��� Ŭ���̾�Ʈ�� �ݿ��Ѵ�.
    //�翬�� ���� �гο��� ����� �ؾ� �ȴ�.
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RpcChangeData(float perfection)
    {
        if (_statePanel == null) return;

        //���� ������ �����Ѵ�.
        Perfection = perfection;

        _statePanel.UpdateState(this);
    }
}
