﻿using Rocket.API.Commands;
using Rocket.Core.Extensions;
using Rocket.Unturned.Player;
using Rocket.Unturned.Utils;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using System.Linq;

namespace Rocket.Unturned.Extensions
{
    public static class CommandArgExtension
    {
        public static bool IsPlayer(this CommandArg arg, out UnturnedPlayer value)
        {
            value = UnturnedPlayer.FromName(arg.RawValue);
            return value != null;
        }
        public static bool IsPlayers(this CommandArg arg, out IEnumerable<UnturnedPlayer> value)
        {
            List<UnturnedPlayer> res = new List<UnturnedPlayer>();
            if (arg.RawValue == "*")
            {
                res.AddRange(ProviderExtension.GetUnturnedPlayers());
            }
            else if (arg.IsPlayer(out var player))
            {
                res.Add(player);
            }
            value = res;
            return value.Count() > 0;
        }
        public static bool IsCSteamID(this CommandArg arg, out CSteamID value)
        {
            value = default;
            if (arg.IsUInt64(out ulong id))
            {
                value = new CSteamID(id);
                return true;
            }
            else
                return false;
        }
        public static bool IsItem(this CommandArg arg, out ItemAsset value)
        {
            if (arg.IsUInt16(out ushort id))
            {
                var asset = Assets.find(EAssetType.ITEM, id);
                if (asset == null)
                {
                    value = null;
                    return false;
                }
                value = (ItemAsset)asset;
                return value != null;
            }
            else
            {
                string search = arg.RawValue.StartsWith("@") ? arg.RawValue.Substring(1) : arg.RawValue;
                value = AssetUtil.GetItemAsset(search);
                return value != null;
            }
        }
    }
}
