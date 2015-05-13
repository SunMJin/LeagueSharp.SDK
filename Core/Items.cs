﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Items.cs" company="LeagueSharp">
//   Copyright (C) 2015 LeagueSharp
//   
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// <summary>
//   Item class used to easily manage items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LeagueSharp.SDK.Core
{
    using System.Collections.Generic;
    using System.Linq;

    using LeagueSharp.CommonEx.Core.Extensions.SharpDX;
    using LeagueSharp.SDK.Core.Wrappers;

    using SharpDX;

    /// <summary>
    ///     Item class used to easily manage items.
    /// </summary>
    public static class Items
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Retruns true if the player has the item and its not on cooldown.
        /// </summary>
        /// <param name="name">
        ///     Name of the Item.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool CanUseItem(string name)
        {
            return
                ObjectManager.Player.InventoryItems.Where(slot => slot.Name == name)
                    .Select(
                        slot =>
                        ObjectManager.Player.Spellbook.Spells.FirstOrDefault(
                            spell => (int)spell.Slot == slot.Slot + (int)SpellSlot.Item1))
                    .Select(inst => inst != null && inst.State == SpellState.Ready)
                    .FirstOrDefault();
        }

        /// <summary>
        ///     Retruns true if the player has the item and its not on cooldown.
        /// </summary>
        /// <param name="id">
        ///     Id of the Item.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool CanUseItem(int id)
        {
            return
                ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id)
                    .Select(
                        slot =>
                        ObjectManager.Player.Spellbook.Spells.FirstOrDefault(
                            spell => (int)spell.Slot == slot.Slot + (int)SpellSlot.Item1))
                    .Select(inst => inst != null && inst.State == SpellState.Ready)
                    .FirstOrDefault();
        }

        /// <summary>
        ///     Returns the ward slot.
        /// </summary>
        /// <returns>
        ///     The <see cref="InventorySlot" />.
        /// </returns>
        public static InventorySlot GetWardSlot()
        {
            var wardIds = new[] { 3340, 3350, 3361, 3154, 2045, 2049, 2050, 2044 };
            return (from wardId in wardIds
                    where CanUseItem(wardId)
                    select ObjectManager.Player.InventoryItems.FirstOrDefault(slot => slot.Id == (ItemId)wardId))
                .FirstOrDefault();
        }

        /// <summary>
        ///     Returns true if the hero has the item.
        /// </summary>
        /// <param name="name">
        ///     Name of the Item.
        /// </param>
        /// <param name="hero">
        ///     Hero to be checked.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool HasItem(string name, Obj_AI_Hero hero = null)
        {
            return (hero ?? ObjectManager.Player).InventoryItems.Any(slot => slot.Name == name);
        }

        /// <summary>
        ///     Returns true if the hero has the item.
        /// </summary>
        /// <param name="id">
        ///     Id of the Item.
        /// </param>
        /// <param name="hero">
        ///     Hero to be checked.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool HasItem(int id, Obj_AI_Hero hero = null)
        {
            return (hero ?? ObjectManager.Player).InventoryItems.Any(slot => slot.Id == (ItemId)id);
        }

        /// <summary>
        ///     Casts the item on the target.
        /// </summary>
        /// <param name="name">
        ///     Name of the Item.
        /// </param>
        /// <param name="target">
        ///     Target to be hit.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool UseItem(string name, Obj_AI_Base target = null)
        {
            return
                ObjectManager.Player.InventoryItems.Where(slot => slot.Name == name)
                    .Select(
                        slot =>
                        target != null
                            ? ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, target)
                            : ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot))
                    .FirstOrDefault();
        }

        /// <summary>
        ///     Casts the item on the target.
        /// </summary>
        /// <param name="id">
        ///     Id of the Item.
        /// </param>
        /// <param name="target">
        ///     Target to be hit.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool UseItem(int id, Obj_AI_Base target = null)
        {
            return
                ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id)
                    .Select(
                        slot =>
                        target != null
                            ? ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, target)
                            : ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot))
                    .FirstOrDefault();
        }

        /// <summary>
        ///     Casts the item on a Vector2 position.
        /// </summary>
        /// <param name="id">
        ///     Id of the Item.
        /// </param>
        /// <param name="position">
        ///     Position of the Item cast.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool UseItem(int id, Vector2 position)
        {
            return UseItem(id, position.ToVector3());
        }

        /// <summary>
        ///     Casts the item on a Vector3 position.
        /// </summary>
        /// <param name="id">
        ///     Id of the Item.
        /// </param>
        /// <param name="position">
        ///     Position of the Item cast.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public static bool UseItem(int id, Vector3 position)
        {
            return position != Vector3.Zero
                   && ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)id)
                          .Select(slot => ObjectManager.Player.Spellbook.CastSpell(slot.SpellSlot, position))
                          .FirstOrDefault();
        }

        #endregion

        /// <summary>
        /// </summary>
        public class Item
        {
            #region Fields

            /// <summary>
            ///     Range of the Item
            /// </summary>
            private float range;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            ///     Initializes a new instance of the <see cref="Item" /> class.
            /// </summary>
            /// <param name="id">
            /// </param>
            /// <param name="range">
            /// </param>
            public Item(int id, float range)
            {
                var item = new ItemData(id);

                // Values
                this.Id = item.Id;
                this.Name = item.Name;
                this.Range = range;
                this.Description = item.PlaintextDescription;
                this.BasePrice = item.BasePrice;
                this.SellPrice = item.SellPrice;
                this.TotalPrice = item.TotalPrice;
                this.Purchaseable = item.Purchaseable;
                this.From = item.From;
                this.Into = item.Into;
                this.Stacks = item.Stacks;
                this.Tags = item.Tags;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the base price.
            /// </summary>
            public int BasePrice { get; private set; }

            /// <summary>
            ///     Gets the description of the Item.
            /// </summary>
            public string Description { get; private set; }

            /// <summary>
            ///     Gets the Id's of the included Items.
            /// </summary>
            public int[] From { get; private set; }

            /// <summary>
            ///     Gets the Id of the Item.
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            ///     Gets the Id of the possible upgraded Item.
            /// </summary>
            public int[] Into { get; private set; }

            /// <summary>
            ///     Gets a value indicating whether is ready.
            /// </summary>
            public bool IsReady
            {
                get
                {
                    return CanUseItem(this.Id);
                }
            }

            /// <summary>
            ///     Gets the Name of the Item
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            ///     Gets a value indicating whether purchaseable.
            /// </summary>
            public bool Purchaseable { get; private set; }

            /// <summary>
            ///     Gets or sets the range.
            /// </summary>
            public float Range
            {
                get
                {
                    return this.range;
                }

                set
                {
                    this.range = value;
                    this.RangeSqr = value * value;
                }
            }

            /// <summary>
            ///     Gets the range sqr.
            /// </summary>
            public float RangeSqr { get; private set; }

            /// <summary>
            ///     Gets the sell price.
            /// </summary>
            public int SellPrice { get; private set; }

            /// <summary>
            ///     Slot of the Item
            /// </summary>
            public List<SpellSlot> Slot
            {
                get
                {
                    return
                        ObjectManager.Player.InventoryItems.Where(slot => slot.Id == (ItemId)this.Id)
                            .Select(slot => slot.SpellSlot)
                            .ToList();
                }
            }

            /// <summary>
            ///     Gets the maximum stacks.
            /// </summary>
            public int Stacks { get; private set; }

            /// <summary>
            ///     Gets the tags.
            /// </summary>
            public string[] Tags { get; private set; }

            /// <summary>
            ///     Gets the total price.
            /// </summary>
            public int TotalPrice { get; private set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     Buys the Items.
            /// </summary>
            public void Buy()
            {
                ObjectManager.Player.BuyItem((ItemId)this.Id);
            }

            /// <summary>
            ///     Casts the Item.
            /// </summary>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public bool Cast()
            {
                return UseItem(this.Id);
            }

            /// <summary>
            ///     Casts the Item on a Target.
            /// </summary>
            /// <param name="target">
            ///     Target as Obj_AI_Base.
            /// </param>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public bool Cast(Obj_AI_Base target)
            {
                return UseItem(this.Id, target);
            }

            /// <summary>
            ///     Casts the Item on a Position.
            /// </summary>
            /// <param name="position">
            ///     Position as Vector2.
            /// </param>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public bool Cast(Vector2 position)
            {
                return UseItem(this.Id, position);
            }

            /// <summary>
            ///     Casts the Item on a Position.
            /// </summary>
            /// <param name="position">
            ///     Position as Vector3.
            /// </param>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public bool Cast(Vector3 position)
            {
                return UseItem(this.Id, position);
            }

            /// <summary>
            ///     Returns if the target is in the range of the Item.
            /// </summary>
            /// <param name="target">
            ///     Target to be checked.
            /// </param>
            /// <returns>
            /// </returns>
            public bool IsInRange(Obj_AI_Base target)
            {
                return this.IsInRange(target.ServerPosition);
            }

            /// <summary>
            ///     Returns if the position is in the range of the Item.
            /// </summary>
            /// <param name="position">
            ///     Position to be checked.
            /// </param>
            /// <returns>
            /// </returns>
            public bool IsInRange(Vector2 position)
            {
                return this.IsInRange(position.ToVector3());
            }

            /// <summary>
            ///     Returns if the position is in the range of the Item.
            /// </summary>
            /// <param name="position">
            ///     Position to be checked.
            /// </param>
            /// <returns>
            /// </returns>
            public bool IsInRange(Vector3 position)
            {
                return ObjectManager.Player.ServerPosition.DistanceSquared(position) < this.RangeSqr;
            }

            /// <summary>
            ///     Returns if the Item is owned.
            /// </summary>
            /// <param name="target">
            ///     Target as Obj_AI_Hero.
            /// </param>
            /// <returns>
            ///     The <see cref="bool" />.
            /// </returns>
            public bool IsOwned(Obj_AI_Hero target = null)
            {
                return HasItem(this.Id, target);
            }

            #endregion
        }
    }
}