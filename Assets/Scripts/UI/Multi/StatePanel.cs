using Custom;
using UnityEngine;

/// <summary>
/// ��ũ��ũ �� �÷��̾� �� ������ ǥ�����ݴϴ�.
/// </summary>
public class StatePanel : MonoBehaviour
{
    [SerializeField] StatePenelEntity[] entities;

    private void Awake()
    {
        entities = GetComponentsInChildren<StatePenelEntity>();
    }

    //�÷��̾ �����
    public bool RegisterPlayer(PlayerDataNetwork player)
    {
        CustomDebug.PrintE($"�÷��̾� ���� ��� {player.NickName}");
        foreach (var entity in entities)
        {
            if (entity.Register(player))
            {
                return true;
            }
        }

        return false;
    }

    //�÷��̾� ���� ��ȭ�� ���� ������Ʈ�� ����
    public void UpdateState(PlayerDataNetwork player)
    {
        foreach (var entity in entities)
        {
            if (entity.UpdateState(player)) break;
        }
    }

    //�÷��̾ �����
    public bool RemovePlayer(PlayerDataNetwork player)
    {
        CustomDebug.PrintE($"�÷��̾� ���� ���� {player.NickName}");
        foreach (var entity in entities)
        {
            if (entity.Remove(player))
            {
                return true;
            }
        }

        return false;
    }
}
