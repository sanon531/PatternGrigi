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

        public DataEntity(DataEntity data)
        {
            type = data.type;
            properties = data.properties;
            _기본값 = data._기본값;
            _배수 = data._배수;
            _증가량 = data._증가량;
            _증가량배수 = data._증가량배수;
            _추가량= data._추가량;

        }



        public static DataEntity OriginalData(int 기본값)
        {
            return new DataEntity(Type.None, 기본값);
        }





    }
}
