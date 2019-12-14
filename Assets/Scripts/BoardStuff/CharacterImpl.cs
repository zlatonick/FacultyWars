using System;
using System.Collections.Generic;
using MetaInfo;

namespace BoardStuff
{
    public class CharacterImpl : Character
    {
        int id;

        int level;

        StuffClass stuffClass;

        int power;

        int startPower;

        Player player;

        List<int> powerHistory;

        BoardStuffManager boardStuffManager;

        List<Action<int>> changePowerActions;

        public CharacterImpl(int id, StuffClass stuffClass, int level, int power,
            Player player, BoardStuffManager boardStuffManager)
        {
            this.id = id;
            this.level = level;
            this.stuffClass = stuffClass;
            this.power = power;
            this.startPower = power;
            this.player = player;
            this.boardStuffManager = boardStuffManager;

            powerHistory = new List<int>();
            changePowerActions = new List<Action<int>>();
        }

        public int GetId()
        {
            return id;
        }

        public int GetLevel()
        {
            return level;
        }

        public void ChangePower(int changeBy)
        {
            power += changeBy;
            boardStuffManager.ChangeCharacterPower(id, power, changeBy);

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