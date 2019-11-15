using System;
using System.Collections.Generic;
using MetaInfo;

namespace BoardStuff
{
    public class CharacterImpl : Character
    {
        StuffClass stuffClass;

        int power;

        int startPower;

        Player player;

        List<int> powerHistory;

        public CharacterImpl(StuffClass stuffClass, int power, Player player)
        {
            this.stuffClass = stuffClass;
            this.power = power;
            this.startPower = power;
            this.player = player;
            powerHistory = new List<int>();
        }

        public void ChangePower(int changeBy)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}