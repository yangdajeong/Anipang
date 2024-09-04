using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class Board : MonoBehaviour //Seaded (봉인된) 더이상 상속이 불가능
{
    public static Board Instance { get; private set; }

    [SerializeField] private AudioClip collectSound;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] Image twinBlock;

    public Row[] rows;

    public Tile[,] Tiles { get; private set; }

    public List<Button> buttons;

    public int Width => Tiles.GetLength(dimension: 0); // x
    public int Height => Tiles.GetLength(dimension: 1); // y

    private List<Tile> _selection = new List<Tile>();

    private const float TweenDuration = 0.25f;

    private void Awake() => Instance = this;

    private void Start()
    {
        Tiles = new Tile[rows.Max(selector: row => row.tiles.Length), rows.Length]; // 타일 배열 초기화

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = rows[y].tiles[x];

                tile.x = x;
                tile.y = y;

                tile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];  

                Tiles[x, y] = rows[y].tiles[x];
            }
        }
        //for문을 다 돌면서 하나씩 빈칸 채워넣기

        Pop();

        for (int i = 0; i < Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                Button button = Tiles[i, j].GetComponent<Button>(); // Tile에서 Button 컴포넌트 가져오기
                buttons.Add(button);
            }
        }
    }

    public async void Select(Tile tile) // 두개의 타일 선택   // async void : 예외 처리와 결과 반환에서 제약이 있기 때문에, 일반적으로 async Task 또는 async Task<T>를 사용하는 것이 좋음.
    {
        if (!_selection.Contains(tile))
        {
            if (_selection.Count > 0) //두번째 선택
            {
                if (Array.IndexOf(_selection[0].Neighbours, tile) != -1) //첫번째 선택한 타일에서 십자가 안에 이웃이 있으면 add
                {
                    _selection.Add(tile);
                }
                else //아니라면 초기화 및 천번째 타일 선택 (첫 타일 선택 후 잊고 다른 타일을 첫번째로 착각했을 시 주변이 안눌리는 문제 방지)
                {
                    _selection.Clear();
                    _selection.Add(tile);
                }
            }
            else
            {
                _selection.Add(tile); //첫번째 선택
            }
        }

        if (_selection.Count < 2) return;

        Debug.Log($"Selected tiles at ({_selection[0].x}, {_selection[0].y}) and ({_selection[1].x}, {_selection[1].y})");

        twinBlock.enabled = true; //스왑일 때는 터치 못하게 막기

        await Swap(_selection[0], _selection[1]); //선택한 타일 두개 서로 바꾸기

        if (CanPop()) //팝할 수 있다
        {
            Pop();
        }
        else //팝할 수 없다
        {
            await Swap(_selection[0], _selection[1]); //원위치로 되돌기
            twinBlock.enabled = false; // 선택 가능하게 하기
        }

        _selection.Clear(); //선택한 타일 없애기
    }

    public async Task Swap(Tile tile1, Tile tile2) // 아이템 서로 바꾸기
    {
        var icon1 = tile1.icon; 
        var icon2 = tile2.icon;

        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();

        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration))
                .Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));

        await sequence.Play()
                    .AsyncWaitForCompletion(); //애니메이션을 실행하고, 그 애니메이션이 완료될 때까지 비동기적으로 대기하는 코드

        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);

        tile1.icon = icon2; //아이템 이미지 바꾸기
        tile2.icon = icon1;

        var tile1Item = tile1.Item;

        tile1.Item = tile2.Item; //아이템 바꿔주기
        tile2.Item = tile1Item;
    }

    private bool CanPop() //팝할 수 있다.
    {
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                if (Tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2)
                    return true;

        return false;
    }

    private async void Pop() //터트리기
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Tiles[x, y];

                var connectedTiles = tile.GetConnectedTiles();

                if (connectedTiles.Skip(1).Count() < 2) continue;

                var deflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    deflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, TweenDuration)); //크기 줄이기
                }

                audioSource.PlayOneShot(collectSound);

                ScoreCounter.Instance.Score += tile.Item.value * connectedTiles.Count; // 점수 올리기

                twinBlock.enabled = true; 

                await deflateSequence.Play()
                                    .AsyncWaitForCompletion();

                var inflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)]; //없어진 자리 다시 랜덤 과일 생기게 하기

                    inflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.one, TweenDuration));
                }

                await inflateSequence.Play()
                                    .AsyncWaitForCompletion();

                x = 0;
                y = 0;

                twinBlock.enabled = false;
            }
        }
    }

    public void pause()
    {
        Time.timeScale = 0;
    }

    public void play()
    {
        Time.timeScale = 1;
    }
}
