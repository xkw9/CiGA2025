using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.API
{
    public interface ILevel
    {
        public Vector2 GetPlayerOriginPos();

        public OriginLocation GetDestination(MovingObject obj);

        void LoadLevel(int level);

        void Win();

    }
}
