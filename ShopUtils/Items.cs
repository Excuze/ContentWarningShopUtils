﻿using ShopUtils.ItemUtils;
using System;
using System.Collections.Generic;
using Zorro.Core;

namespace ShopUtils
{
    public class Items
    {
        internal static List<Item> registerItems = new List<Item>();
        internal static List<Item> registerSpawnableItem = new List<Item>();

        ///<summary>
        ///Add Shop Item
        /// </summary>
        public static void RegisterShopItem(Item item, ShopItemCategory category = ShopItemCategory.Invalid, int price = -1)
        {
            if (price >= 0)
            {
                item.price = price;
            }

            if (category != ShopItemCategory.Invalid)
            {
                item.Category = category;
            }

            if (item.Category == ShopItemCategory.Invalid)
            {
                throw new ShopUtilsException(
                    $"Error Category Invalid \n" +
                    $"Items Name: {item.displayName} \n"
                    );
            }

            if (item.price < 0)
            {
                throw new ShopUtilsException(
                    $"Error Item Price \n" +
                    $"Items Name: {item.displayName} \n"
                    );
            }

            item.purchasable = true;

            if (!registerItems.Contains(item)) {
                registerItems.Add(item);
            }
        }

        ///<summary>
        ///Add Spawnable Item
        /// </summary>
        public static void RegisterSpawnableItem(Item item, float Rarity = 1, int BudgetCost = 1)
        {
            // Artifact
            item.rarity = Rarity;
            item.budgetCost = BudgetCost;

            if (!registerSpawnableItem.Contains(item)) {
                registerSpawnableItem.Add(item);
            }
        }

        ///<summary>
        ///Remove Spawnable Item
        /// </summary>
        public static void RemoveSpawnableItem(Item item)
        {
            if (registerSpawnableItem.Contains(item)) {
                registerSpawnableItem.Remove(item);
            }
        }

        ///<summary>
        ///Remove Item From Shop
        /// </summary>
        public static void UnRegisterItem(Item item)
        {
            if (registerItems.Contains(item)) {
                registerItems.Remove(item);
            }
        }

        ///<summary>
        ///Init All Items Id and Guid
        /// </summary>
        internal static void InitAllItems()
        {
            registerItems.ForEach(item => item.id = 0);
            registerItems.ForEach(item =>
            {
                item.id = GetMaxItemID();

                UtilsLogger.LogInfo(
                    $"Item: {item.displayName}, " +
                    $"Buyable: {item.purchasable}, " +
                    $"Spawnable: {item.spawnable}, " +
                    $"ItemId: {item.id}, " +
                    $"Guid: {item.persistentID}"
                    );
            });
        }

        private static byte GetMaxItemID()
        {
            int id = 0;
            foreach (Item item in SingletonAsset<ItemDatabase>.Instance.Objects)
            {
                id = Math.Max(item.id, id);
            }

            foreach (Item item in registerItems)
            {
                id = Math.Max(item.id, id);
            }

            return (byte) (id + 1);
        }
    }
}
