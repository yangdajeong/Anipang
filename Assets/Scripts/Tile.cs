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
            if (_item == value) // 현재 아이템과 새 아이템이 같으면 아무 작업도 하지 않음
                return;

            _item = value;

            icon.sprite = _item.sprite;
        }
    }

    public Image icon;
    public Button button;

    //테두리 타일들은 일정 반환 못하게
    public Tile Left => x > 0 ? Board.Instance.Tiles[x - 1, y] : null; // 현재 타일의 왼쪽 이웃 타일을 반환. 좌표가 0보다 클 때만 반환
    public Tile Top => y > 0 ? Board.Instance.Tiles[x, y - 1] : null; // 현재 타일의 위쪽 이웃 타일을 반환. 좌표가 0보다 클 때만 반환
    public Tile Right => x < Board.Instance.Width - 1 ? Board.Instance.Tiles[x + 1, y] : null; // 현재 타일의 오른쪽 이웃 타일을 반환. 좌표가 보드의 너비보다 작을 때만 반환
    public Tile Botton => y < Board.Instance.Height - 1 ? Board.Instance.Tiles[x, y + 1] : null; // 현재 타일의 아래쪽 이웃 타일을 반환. 좌표가 보드의 높이보다 작을 때만 반환

    public Tile[] Neighbours => new[] // 현재 타일의 모든 이웃 타일을 배열로 반환
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

    public List<Tile> GetConnectedTiles(List<Tile> exclude = null) // 현재 타일과 같은 아이템을 가진 연결된 모든 타일들을 재귀적으로 찾아서 반환
    {
        var result = new List<Tile> { this }; // 결과 리스트를 초기화하고 현재 타일을 포함

        if (exclude == null) // exclude가 null인 경우, 현재 타일을 포함하는 새 리스트로 초기화
        {
            exclude = new List<Tile> { this };
        }
        else
        {
            exclude.Add(item: this); // 그렇지 않으면 현재 타일을 exclude 리스트에 추가
        }

        foreach (var neighbour in Neighbours) // 현재 타일의 이웃들을 탐색
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != Item) continue; // 이웃이 null이거나 이미 방문한 타일이거나, 아이템이 다른 경우 탐색을 건너뜀

            result.AddRange(neighbour.GetConnectedTiles(exclude)); // 유효한 이웃 타일에 대해 재귀적으로 연결된 타일들을 찾고 결과에 추가 // AddRange : 리스트에 다른 리스트의 모든 요소를 추가
        }

        return result;
    }
}
