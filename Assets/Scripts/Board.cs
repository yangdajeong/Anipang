using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class Board : MonoBehaviour //Seaded (���ε�) ���̻� ����� �Ұ���
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
        Tiles = new Tile[rows.Max(selector: row => row.tiles.Length), rows.Length]; // Ÿ�� �迭 �ʱ�ȭ

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
        //for���� �� ���鼭 �ϳ��� ��ĭ ä���ֱ�

        Pop();

        for (int i = 0; i < Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                Button button = Tiles[i, j].GetComponent<Button>(); // Tile���� Button ������Ʈ ��������
                buttons.Add(button);
            }
        }
    }

    public async void Select(Tile tile) // �ΰ��� Ÿ�� ����   // async void : ���� ó���� ��� ��ȯ���� ������ �ֱ� ������, �Ϲ������� async Task �Ǵ� async Task<T>�� ����ϴ� ���� ����.
    {
        if (!_selection.Contains(tile))
        {
            if (_selection.Count > 0) //�ι�° ����
            {
                if (Array.IndexOf(_selection[0].Neighbours, tile) != -1) //ù��° ������ Ÿ�Ͽ��� ���ڰ� �ȿ� �̿��� ������ add
                {
                    _selection.Add(tile);
                }
                else //�ƴ϶�� �ʱ�ȭ �� õ��° Ÿ�� ���� (ù Ÿ�� ���� �� �ذ� �ٸ� Ÿ���� ù��°�� �������� �� �ֺ��� �ȴ����� ���� ����)
                {
                    _selection.Clear();
                    _selection.Add(tile);
                }
            }
            else
            {
                _selection.Add(tile); //ù��° ����
            }
        }

        if (_selection.Count < 2) return;

        Debug.Log($"Selected tiles at ({_selection[0].x}, {_selection[0].y}) and ({_selection[1].x}, {_selection[1].y})");

        twinBlock.enabled = true; //������ ���� ��ġ ���ϰ� ����

        await Swap(_selection[0], _selection[1]); //������ Ÿ�� �ΰ� ���� �ٲٱ�

        if (CanPop()) //���� �� �ִ�
        {
            Pop();
        }
        else //���� �� ����
        {
            await Swap(_selection[0], _selection[1]); //����ġ�� �ǵ���
            twinBlock.enabled = false; // ���� �����ϰ� �ϱ�
        }

        _selection.Clear(); //������ Ÿ�� ���ֱ�
    }

    public async Task Swap(Tile tile1, Tile tile2) // ������ ���� �ٲٱ�
    {
        var icon1 = tile1.icon; 
        var icon2 = tile2.icon;

        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;

        var sequence = DOTween.Sequence();

        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TweenDuration))
                .Join(icon2Transform.DOMove(icon1Transform.position, TweenDuration));

        await sequence.Play()
                    .AsyncWaitForCompletion(); //�ִϸ��̼��� �����ϰ�, �� �ִϸ��̼��� �Ϸ�� ������ �񵿱������� ����ϴ� �ڵ�

        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);

        tile1.icon = icon2; //������ �̹��� �ٲٱ�
        tile2.icon = icon1;

        var tile1Item = tile1.Item;

        tile1.Item = tile2.Item; //������ �ٲ��ֱ�
        tile2.Item = tile1Item;
    }

    private bool CanPop() //���� �� �ִ�.
    {
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                if (Tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2)
                    return true;

        return false;
    }

    private async void Pop() //��Ʈ����
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
                    deflateSequence.Join(connectedTile.icon.transform.DOScale(Vector3.zero, TweenDuration)); //ũ�� ���̱�
                }

                audioSource.PlayOneShot(collectSound);

                ScoreCounter.Instance.Score += tile.Item.value * connectedTiles.Count; // ���� �ø���

                twinBlock.enabled = true; 

                await deflateSequence.Play()
                                    .AsyncWaitForCompletion();

                var inflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)]; //������ �ڸ� �ٽ� ���� ���� ����� �ϱ�

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
