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
        private DataEntity(Type type, int �⺻��, Property properties)
        {
            this.type = type;
            _�⺻�� = �⺻��;
            this.properties = properties;
        }


        public static DataEntity ����������(int �⺻��)
        {
            return new DataEntity(Type.����, �⺻��);
        }





    }
}