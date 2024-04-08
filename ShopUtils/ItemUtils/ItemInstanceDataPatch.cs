﻿using HarmonyLib;
using ShopUtils;
using System;

namespace Boombox.ItemUtils
{
    [HarmonyPatch(typeof(ItemInstanceData))]
    public class ItemInstanceDataPatch
    {
        private static int EntryCount = 9;

        [HarmonyPrefix]
        [HarmonyPatch(nameof(ItemInstanceData.GetEntryIdentifier))]
        public static bool GetEntryIdentifier(ref byte __result, Type type)
        {
            if (!Entries.registerEntries.Contains(type)) {
                return true;
            }

            int begin = EntryCount;
            foreach (Type type1 in Entries.registerEntries)
            {
                begin += 1;
                if (type1 == type)
                {
                    __result = (byte)begin;
                    return false;
                }
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(ItemInstanceData.GetEntryType))]
        public static bool GetEntryType(ref ItemDataEntry __result, byte identifier)
        {
            int begin = EntryCount;
            foreach (Type type1 in Entries.registerEntries)
            {
                begin += 1;
                if (identifier == (byte) begin)
                {
                    __result = (ItemDataEntry) type1.GetConstructor(new Type[0]).Invoke(null);
                    return false;
                }
            }

            return true;
        }
    }
}
