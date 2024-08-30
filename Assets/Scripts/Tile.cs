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

        if (exclude == null) //exclude가 null인 경우, 현재 타일을 포함하는 새 리스트로 초기화
        {
            exclude = new List<Tile> { this, };
        }
        else
        {
            exclude.Add(item: this); //그렇지 않으면 현재 타일을 exclude 리스트에 추가
        }
        //재귀 호출 시, 동일한 타일을 다시 방문하지 않도록 하기 위함입니다.


        foreach (var neighbour in Neighbours) // Neighbours 현재 타일에 인접한 타일들
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item) continue; // 이웃한 타일이 없음, 이미 검사한 타일, 이웃 아이템이랑 다름 (하나라도 충족하면 continue로 재귀함수 끝)

            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }

        // 현재 타일과 같은 아이템을 가진 연결된 모든 타일들을 재귀적으로 찾아서 반환.exclude 리스트를 사용하여 이미 방문한 타일을 제외함으로써 무한 루프를 방지.

        return result;
    }
}
