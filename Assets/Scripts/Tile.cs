using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tile : MonoBehaviour
{
    public int x;
    public int y; 

    private Item _item; 

    public Item Item
    {
        get => _item;

        set
        {
            if (_item == value) // ���� �����۰� �� �������� ������ �ƹ� �۾��� ���� ����
                return;

            _item = value;

            icon.sprite = _item.sprite;
        }
    }

    public Image icon;
    public Button button;

    //�׵θ� Ÿ�ϵ��� ���� ��ȯ ���ϰ�
    public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null; // ���� Ÿ���� ���� �̿� Ÿ���� ��ȯ. ��ǥ�� 0���� Ŭ ���� ��ȯ
    public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null; // ���� Ÿ���� ���� �̿� Ÿ���� ��ȯ. ��ǥ�� 0���� Ŭ ���� ��ȯ
    public Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null; // ���� Ÿ���� ������ �̿� Ÿ���� ��ȯ. ��ǥ�� ������ �ʺ񺸴� ���� ���� ��ȯ
    public Tile Botton => y < Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null; // ���� Ÿ���� �Ʒ��� �̿� Ÿ���� ��ȯ. ��ǥ�� ������ ���̺��� ���� ���� ��ȯ

    public Tile[] Neighbours => new[] // ���� Ÿ���� ��� �̿� Ÿ���� �迭�� ��ȯ
    {
        Left,
        Top,
        Right,
        Botton
    };

    private void Start()
    {
        button.onClick.AddListener(() => Board.Instance.Select(tile: this));
    }

    public List<Tile> GetConnectedTiles(List<Tile> exclude = null) // ���� Ÿ�ϰ� ���� �������� ���� ����� ��� Ÿ�ϵ��� ��������� ã�Ƽ� ��ȯ
    {
        var result = new List<Tile> { this }; // ��� ����Ʈ�� �ʱ�ȭ�ϰ� ���� Ÿ���� ����

        if (exclude == null) // exclude�� null�� ���, ���� Ÿ���� �����ϴ� �� ����Ʈ�� �ʱ�ȭ
        {
            exclude = new List<Tile> { this };
        }
        else
        {
            exclude.Add(item: this); // �׷��� ������ ���� Ÿ���� exclude ����Ʈ�� �߰�
        }

        foreach (var neighbour in Neighbours) // ���� Ÿ���� �̿����� Ž��
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item) continue; // �̿��� null�̰ų� �̹� �湮�� Ÿ���̰ų�, �������� �ٸ� ��� Ž���� �ǳʶ�

            result.AddRange(neighbour.GetConnectedTiles(exclude)); // ��ȿ�� �̿� Ÿ�Ͽ� ���� ��������� ����� Ÿ�ϵ��� ã�� ����� �߰� // AddRange : ����Ʈ�� �ٸ� ����Ʈ�� ��� ��Ҹ� �߰�
        }

        return result;
    }
}
