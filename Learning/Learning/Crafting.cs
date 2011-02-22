using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning
{
    class Crafting
    {
        public static List<Recipe> recipes = new List<Recipe>();

        public static Recipe getRecipe(Item[] items)
        {
            foreach (Recipe recipe in recipes)
            {
                if (recipe.canCraft(items) > 0) { return recipe; }
            }
            return null;
        }
        public static Item craft(Item[] items,int amount)
        {
            Recipe toCraft = getRecipe(items);
            if (toCraft == null) { return null; }
            for(int i = 0; i<items.Length; i++){
                items[i].amount -= toCraft.requiredItems[i].amount * amount;
                if (items[i].amount <= 0) { items[i] = null; }
            }
            return toCraft.craft(amount);
        }
        public static void addRecipe(Item[] items, Item craft){
            recipes.Add(new Recipe(items, craft));
        }
    }
    class Recipe
    {
        public readonly Item[] requiredItems;
        public readonly Item craftedItem;
        public Recipe(Item[] reqI, Item cI)
        {
            this.craftedItem = cI;
            this.requiredItems = reqI;
        }
        public int canCraft(Item[] items)
        {
            if (items.Length != requiredItems.Length)
            {
                return 0;
            }
            int minAmount = -1;
            int temp;
            for (int i = 0; i < items.Length; i++)
            {
                if ((items[i] == null || requiredItems == null))
                {
                    if (items[i] != requiredItems[i]) { return 0; }
                }
                else
                {
                    if (items[i].type != requiredItems[i].type) { return 0; }
                    temp = items[i].amount / requiredItems[i].amount;
                    if (temp < minAmount || minAmount == -1) { minAmount = temp; }
                }
            }
            if (minAmount == -1) { return 0; }
            return minAmount;
        }
        public Item craft(int amount)
        {
            Item crafted = new Item(craftedItem.type,amount);
            return crafted;
        }
    }
}
