﻿// <copyright file="Turret.cs" company="LeagueSharp">
//    Copyright (c) 2015 LeagueSharp.
// 
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
// 
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
// 
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see http://www.gnu.org/licenses/
// </copyright>

namespace LeagueSharp.SDK
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Turret tracker and event handler.
    /// </summary>
    public static partial class Events
    {
        #region Static Fields

        /// <summary>
        ///     The Turrets list.
        /// </summary>
        private static readonly IDictionary<Obj_AI_Turret, TurretArgs> Turrets =
            new Dictionary<Obj_AI_Turret, TurretArgs>();

        #endregion

        #region Public Events

        /// <summary>
        ///     On turret attack event.
        /// </summary>
        public static event EventHandler<TurretArgs> OnTurretAttack;

        #endregion

        #region Methods

        /// <summary>
        ///     On Create event.
        /// </summary>
        /// <param name="sender">
        ///     The sender
        /// </param>
        private static void EventTurret(GameObject sender)
        {
            if (sender.Type == GameObjectType.obj_GeneralParticleEmitter && sender.Name.Contains("Turret"))
            {
                var turret = Turrets.OrderBy(t => t.Key.Distance(sender.Position)).FirstOrDefault().Value;
                if (turret != null)
                {
                    turret.TurretBoltObject = sender;
                }
            }
        }

        /// <summary>
        ///     On process spell cast event.
        /// </summary>
        /// <param name="sender">
        ///     The sender
        /// </param>
        private static void EventTurret(Obj_AI_Base sender)
        {
            Obj_AI_Turret[] turret = { sender as Obj_AI_Turret };
            if (turret[0] != null)
            {
                if (Turrets.Count == 0)
                {
                    foreach (var gameObjectTurret in
                        GameObjects.Turrets)
                    {
                        Turrets.Add(gameObjectTurret, new TurretArgs { Turret = gameObjectTurret });
                        if (gameObjectTurret.NetworkId == turret[0].NetworkId)
                        {
                            turret[0] = gameObjectTurret;
                        }
                    }
                }
                else
                {
                    foreach (var gameObjectTurret in
                        Turrets.Where(gameObjectTurret => gameObjectTurret.Key.NetworkId == turret[0].NetworkId))
                    {
                        turret[0] = gameObjectTurret.Key;
                    }
                }

                Turrets[turret[0]].AttackStart = Variables.TickCount;
                if (Turrets[turret[0]].Target != null && Turrets[turret[0]].Target.IsValid)
                {
                    Turrets[turret[0]].AttackDelay = (turret[0].AttackCastDelay * 1000)
                                                     + (turret[0].Distance(Turrets[turret[0]].Target)
                                                        / turret[0].BasicAttack.MissileSpeed * 1000);
                    Turrets[turret[0]].AttackEnd = (int)(Variables.TickCount + Turrets[turret[0]].AttackDelay);
                }

                OnTurretAttack?.Invoke(turret[0], Turrets[turret[0]]);
            }
        }

        private static void EventTurretConstruct()
        {
            OnLoad += (sender, args) =>
                {
                    foreach (var turret in GameObjects.Turrets)
                    {
                        Turrets.Add(turret, new TurretArgs { Turret = turret });
                    }
                };
        }

        #endregion
    }

    /// <summary>
    ///     Turret event data which are passed with <see cref="Events.OnTurretAttack" />
    /// </summary>
    public class TurretArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the attack delay.
        /// </summary>
        public float AttackDelay { get; set; }

        /// <summary>
        ///     Gets or sets the attack end.
        /// </summary>
        public int AttackEnd { get; set; }

        /// <summary>
        ///     Gets or sets the attack start.
        /// </summary>
        public int AttackStart { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the turret is winding up.
        /// </summary>
        public bool IsWindingUp => this.Turret.IsWindingUp;

        /// <summary>
        ///     Gets the target.
        /// </summary>
        public AttackableUnit Target => this.Turret?.Target;

        /// <summary>
        ///     Gets or sets the turret.
        /// </summary>
        public Obj_AI_Turret Turret { get; set; }

        /// <summary>
        ///     Gets or sets the turret bolt object.
        /// </summary>
        public GameObject TurretBoltObject { get; set; }

        #endregion
    }
}