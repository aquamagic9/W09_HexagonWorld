using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CraftingManager : Singleton<CraftingManager>
{
    public string[] recipes;
    public GameObject[] recipeResults;

    public GameObject TargetPositionTileToRecipeResult(int targetX, int targetY)
    {
        return ReturnRecipeResult(ItemListsToStringRecipe(TileMapManager.Instance.ReturnAroundSixItems(targetX, targetY)));
    }

    public GameObject ReturnRecipeResult(string recipe)
    {
        int i = 0;
        foreach(var rec in recipes)
        {
            if (rec.Equals(recipe))
            {
                return recipeResults[i];
            }
            i++;
        }
        return null;
    }

    public string ItemListsToStringRecipe(List<GameObject> itemLists)
    {
        var lists = itemLists.OrderBy(x => x.GetComponent<ItemPos>().name);
        List<GameObject> orderedList = lists.ToList<GameObject>();
        string recipe = "";
        int count = 1;
        for (int i = 1; i <= orderedList.Count; i++)
        {
            if (i == orderedList.Count)
            {
                recipe += (orderedList[i - 1].GetComponent<ItemPos>().name + count);
                continue;
            }
            if (orderedList[i - 1].GetComponent<ItemPos>().name.Equals(orderedList[i].GetComponent<ItemPos>().name))
            {
                count++;
            }
            else
            {
                recipe += (orderedList[i - 1].GetComponent<ItemPos>().name + count);
                count = 1;
            }
        }

        return recipe;
    }
}
