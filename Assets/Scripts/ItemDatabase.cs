using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase
{
    public static Item[] Items { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void Initialize() => Items = Resources.LoadAll<Item>(path: "Items/");
}
