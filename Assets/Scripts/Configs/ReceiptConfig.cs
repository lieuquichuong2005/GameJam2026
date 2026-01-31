using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Recipe/Stove Recipe")]
public class StoveRecipe : ScriptableObject
{
    public List<ItemType> requiredItemIds;
}