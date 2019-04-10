using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusNodeGenerator : MonoBehaviour
{
    private const string prefab_pass = "UnitStatus";

    public static List<UnitStateUI> playerUnitNodes { get; private set; } = new List<UnitStateUI>();
    public static List<UnitStateUI> opponentUnitNodes { get; private set; } = new List<UnitStateUI>();

    private static StatusNodeGenerator singleton;
    private static Transform playerNodeContent;
    private static Transform opponentNodeContent;

    private void Awake()
    {
        InitializeStatusNode();

        singleton = this;
        playerNodeContent = singleton.transform.Find("YourUnit/ScrollView/Content");
        opponentNodeContent = singleton.transform.Find("EnemyUnit/ScrollView/Content");
        

    }


    public static UnitStateUI GenerateStateNode(bool playerUnit, UnitBase unitBase)
    {

        if (playerUnit)
        {
            GameObject instance = InstantiateStatusNode(playerNodeContent);
            UnitStateUI node = new UnitStateUI(instance, unitBase);

            playerUnitNodes.Add(node);
            return node;
        }
        else
        {
            GameObject instance = InstantiateStatusNode(opponentNodeContent);
            UnitStateUI node = new UnitStateUI(instance, unitBase);

            opponentUnitNodes.Add(node);
            return node;
        }

    }

    public static GameObject InstantiateStatusNode(Transform parent)
    {
        return Instantiate(Resources.Load(prefab_pass), parent) as GameObject;
    }
    
    public static void InitializeStatusNode()
    {
        //プレイヤー側の初期化
        foreach(UnitStateUI node in playerUnitNodes)
        {
            Destroy(node.instance);
        }
        playerUnitNodes.Clear();

        //プレイヤー側の初期化
        foreach (UnitStateUI node in opponentUnitNodes)
        {
            Destroy(node.instance);
        }
        opponentUnitNodes.Clear();
    }

}
