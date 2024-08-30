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
            if (_item == value)
                return;

            _item = value;

            icon.sprite = _item.sprite;
        }
    }

    public Image icon;

    public Button button;


    public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null;
    public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null;
    public Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null;
    public Tile Botton => y < Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null;

    public Tile[] Neighbours => new[]
    {
        Left,
        Top,
        Right,
        Botton
    };


    private void Start() => button.onClick.AddListener(call: () => Board.Instance.Select(tile: this));

    public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    {
        var result = new List<Tile> { this, };

        if (exclude == null) //exclude�� null�� ���, ���� Ÿ���� �����ϴ� �� ����Ʈ�� �ʱ�ȭ
        {
            exclude = new List<Tile> { this, };
        }
        else
        {
            exclude.Add(item: this); //�׷��� ������ ���� Ÿ���� exclude ����Ʈ�� �߰�
        }
        //��� ȣ�� ��, ������ Ÿ���� �ٽ� �湮���� �ʵ��� �ϱ� �����Դϴ�.


        foreach (var neighbour in Neighbours) // Neighbours ���� Ÿ�Ͽ� ������ Ÿ�ϵ�
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item) continue; // �̿��� Ÿ���� ����, �̹� �˻��� Ÿ��, �̿� �������̶� �ٸ� (�ϳ��� �����ϸ� continue�� ����Լ� ��)

            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }

        // ���� Ÿ�ϰ� ���� �������� ���� ����� ��� Ÿ�ϵ��� ��������� ã�Ƽ� ��ȯ.exclude ����Ʈ�� ����Ͽ� �̹� �湮�� Ÿ���� ���������ν� ���� ������ ����.

        return result;
    }
}
