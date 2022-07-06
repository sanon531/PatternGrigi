using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToronPuzzle
{
    public partial class DataEntity
    {
        public DataEntity(Type type, int 기본값)
        {
            this.type = type;
            _기본값 = 기본값;
        }
        private DataEntity(Type type, int 기본값, Property properties)
        {
            this.type = type;
            _기본값 = 기본값;
            this.properties = properties;
        }


        public static DataEntity 고유데이터(int 기본값)
        {
            return new DataEntity(Type.없음, 기본값);
        }





    }
}
