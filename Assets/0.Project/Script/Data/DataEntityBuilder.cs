using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public partial class DataEntity
    {
        public DataEntity(Type type, int �⺻��)
        {
            this.type = type;
            _�⺻�� = �⺻��;
        }
        public DataEntity(Type type, float �⺻��)
        {
            this.type = type;
            _�⺻�� = �⺻��;
        }

        private DataEntity(Type type, int �⺻��, Property properties)
        {
            this.type = type;
            _�⺻�� = �⺻��;
            this.properties = properties;
        }

        public DataEntity(DataEntity data)
        {
            type = data.type;
            properties = data.properties;
            _�⺻�� = data._�⺻��;
            _��� = data._���;
            _������ = data._������;
            _��������� = data._���������;
            _�߰���= data._�߰���;

        }



        public static DataEntity OriginalData(int �⺻��)
        {
            return new DataEntity(Type.None, �⺻��);
        }





    }
}