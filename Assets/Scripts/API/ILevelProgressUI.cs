using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.API
{
    public interface ILevelProgressUI
    {

        public void LoadLevelInfo(int level, int amountNeed);

        public void RefreshCurAmount(int amount);

        public void startCountDown(int seconds);

        public void forceGoToNextLevel();
    }
}
