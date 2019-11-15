using System;
using System.Collections.Generic;
using MetaInfo;

namespace BoardStuff
{
    public interface Character
    {
        int GetPower();

        int GetStartPower();

        StuffClass GetStuffClass();

        Player GetPlayer();

        List<int> GetPowerHistory();

        void ChangePower(int changeBy);

        void SetChangePowerAction(Action<int> action);
    }
}
