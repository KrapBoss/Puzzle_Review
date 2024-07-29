using Custom;
using Fusion;
using UnityEngine;

//�÷��̾� ĳ���͸� �����մϴ�.
public class PlayerChracterSpawner : NetworkBehaviour
{
    [Networked] private bool gameIsReady { get; set; }

    [SerializeField] NetworkPrefabRef character;

    // �� ���ÿ��� �÷��̾��� ������ ���¸� ��Ÿ��.
    bool spawned;

    //�� ���ÿ��� ȣ���մϴ�.
    public override void Spawned()
    {
        //���� �غ� �Ǿ������� ĳ���͸� �ٷ� �����մϴ�.
        if (gameIsReady)
        {
            CreateCharacter();
        }
    }

    //��Ʈ��ũ ���ῡ ���� �غ� ���¸� ���޹޴´�.
    public void ReadyGame()
    {
        gameIsReady = true;
        RPCCreateChracter();
    }


    //Host�� ������
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPCCreateChracter()
    {
        CreateCharacter();
    }

    public void CreateCharacter()
    {
        if (spawned) return;

        CustomDebug.Print("�÷��̾� ĳ���� ����");
        NetworkObject go = Runner.Spawn(character, Vector3.zero, Quaternion.identity);
        //�ش� ��ü�� ������ �÷��̾�� �ο��ϱ� ���� ��
        Runner.SetPlayerObject(Runner.LocalPlayer, go);

        spawned = true;
    }
}
