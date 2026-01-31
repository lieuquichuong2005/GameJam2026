using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Config/Inventory/MergeDatabase")]
public class ItemMergeDatabase : ScriptableObject
{
    [System.Serializable]
    public class MergeRule
    {
        public InventoryItem a;
        public InventoryItem b;
        public InventoryItem result;
    }

    public List<MergeRule> rules;

    public bool CanMerge(InventoryItem a, InventoryItem b)
    {
        Debug.Log("CanMerge");
        
        return rules.Exists(r =>
            (r.a == a && r.b == b) ||
            (r.a == b && r.b == a)
        );
    }

    public InventoryItem GetResult(InventoryItem a, InventoryItem b)
    {
        var rule = rules.Find(r =>
            (r.a == a && r.b == b) ||
            (r.a == b && r.b == a)
        );

        return rule?.result;
    }
}