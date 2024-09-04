using UnityEngine;

public class ItemDatabase
{
    public static Item[] Items { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() => Items = Resources.LoadAll<Item>(path: "Items/"); //BeforeSceneLoad은 Awake와 Start전에 실행
}
