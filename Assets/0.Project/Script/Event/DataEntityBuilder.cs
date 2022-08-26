using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public partial class DataEntity
    {
        public DataEntity(Type type, int 기본값)
        {
            this.type = type;
            _기본값 = 기본값;
        }
        public DataEntity(Type type, float 기본값)
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




        public static DataEntity OriginalData(int 기본값)
        {
            return new DataEntity(Type.None, 기본값);
        }





    }
}
