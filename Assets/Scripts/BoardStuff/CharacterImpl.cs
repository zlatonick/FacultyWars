﻿using System;
using System.Collections.Generic;
using MetaInfo;

namespace BoardStuff
{
    public class CharacterImpl : Character
    {
        int id;

        StuffClass stuffClass;

        int power;

        int startPower;

        Player player;

        List<int> powerHistory;

        CharacterManager characterManager;

        List<Action<int>> changePowerActions;

        public CharacterImpl(int id, StuffClass stuffClass, int power,
            Player player, CharacterManager characterManager)
        {
            this.id = id;
            this.stuffClass = stuffClass;
            this.power = power;
            this.startPower = power;
            this.player = player;
            this.characterManager = characterManager;

            powerHistory = new List<int>();
            changePowerActions = new List<Action<int>>();
        }

        public int GetId()
        {
            return id;
        }

        public void ChangePower(int changeBy)
        {
            power += changeBy;
            characterManager.ChangeCharacterPower(id, power);

            foreach (Action<int> action in changePowerActions)
            {
                action(changeBy);
            }
        }

        public Player GetPlayer()
        {
            return player;
        }

        public int GetPower()
        {
            return power;
        }

        public List<int> GetPowerHistory()
        {
            return powerHistory;
        }

        public int GetStartPower()
        {
            return startPower;
        }

        public StuffClass GetStuffClass()
        {
            return stuffClass;
        }

        public void SetChangePowerAction(Action<int> action)
        {
            changePowerActions.Add(action);
        }
    }
}