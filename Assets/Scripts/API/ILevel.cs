using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.API
{
    public interface ILevel
    {
        public Vector2 GetPlayerOriginPos();

        public OriginLocation GetDestination(MovingObject obj);

        public void LoadLevel(int level, List<MovingObject> movingObjList, List<OriginLocation> originLocationList);

        public void Win();

        public void addFinishObject(string tag, MovingObject obj);

        public void removeFinishObject(string tag, MovingObject obj);

        public List<MovingObject> GetMovingObjects();

    }
}
