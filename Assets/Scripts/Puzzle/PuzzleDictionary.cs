using Custom;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ��� ���� ���� ������ �����ϰ� ������ �ִ´�.
/// ������ �� ���ȴ�.
/// </summary>
public class PuzzleDictionary : MonoBehaviour
{
    #region Instance
    static PuzzleDictionary _instance;
    public static PuzzleDictionary Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PuzzleDictionary>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("PuzzlePieceFactory");
                    _instance = go.AddComponent<PuzzleDictionary>();
                }
            }
            return _instance;
        }
    }
    #endregion

    private Dictionary<string, List<Piece>> dictionary = new Dictionary<string, List<Piece>>();


    //���ϴ� ���� �� �ϳ�
    public Piece GetPiece(string name)
    {
        if (!dictionary.ContainsKey(name))
        {
            return null;
        }

        Piece[] p = dictionary[name].Where(x => !x.data.Activation).ToArray();

        return p != null ? p[0] : null;
    }

    //���� �߰�
    public void AddPiece(string name, Piece data)
    {
        if (data == null)
        {
            Debug.LogError($"{name}�� �ش��ϴ� �������� �����ϴ�.");
        }

        if (!dictionary.ContainsKey(name)) dictionary[name] = new List<Piece> { data };
        else dictionary[name].Add(data);

        //CustomDebug.PrintW($"{name}��� {dictionary[name].Count} {data.name}");
    }

    public PieceData[] GetAllPieceData()
    {
        return dictionary.SelectMany(kv => kv.Value).Select(x => x.data).ToArray();
    }

    //�ߺ����� ���� ���� ���� �����͸� �Ѱ��� ��ȯ
    public PieceData[] GetVariablePieceData()
    {
        if (dictionary.Count == 0) return null;

        return dictionary.Values.Select(x => x[0].data).ToArray();
    }

    //�ش� �̸��� ���� ��� ���� ������ ��ȯ�մϴ�.
    public Piece[] GetPieceWithName(string name)
    {
        if (!dictionary.ContainsKey(name)) return null;
        return dictionary[name].ToArray();
    }

    //�ش� �ǽ��� ���� ���� ��
    public int GetPieceCount(string name)
    {
        if (dictionary.Count == 0) return 0;

        if (!dictionary.ContainsKey(name)) { Debug.LogWarning($"{name} �� �ش��ϴ� ���� �����ϴ�."); return 0; }

        int length = dictionary[name].Where(x => !x.data.Activation).ToArray().Length;

        return length;
    }
    //��� ���� ����
    public void Clear()
    {
        if (dictionary.Count == 0) { CustomDebug.PrintE("���� ������ �����Ͱ� �����ϴ�."); return; }

        foreach (var p in dictionary.Values)
        {
            p.Clear();
        }

        dictionary.Clear();
    }
}
