using System;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    [System.Serializable]
    public partial class DataEntity
    {
        public void ResetAdd() { _������ = 0; }
        public void Add������(float amount) { _������ += amount; }
        public void Add���������(float amount) { _��������� *= amount; }
        public void Add���(float amount) { _��� *= amount; }
        public void Add�߰���(int amount) { _�߰��� += amount; }


        private float _�⺻�� = 0;
        private float _������ = 0;
        private float _��������� = 1f;  //���������� ���Ѵ�. (���ݷ� ��� � ���)
        private float _��� = 1f;        //�⺻���� �������� ������ ���� ���Ѵ�.
        private float _�߰��� = 0;         //������ ����� �� �Ϸ� �� ��, ���� �߰��Ѵ�.
        public float FinalValue { get {
                //Debug.Log("�⺻��: " + _�⺻�� + ", ������" + _������ +", ������ ���"+ _��������� + ", �߰��� "+ _�߰���+" , ���" +_���);
                //Debug.Log(" a"+(float)((_�⺻�� + _������ * _���������) * _���));
                return (float)((_�⺻�� + _������ * _���������) * _��� + _�߰���); 
            
            } }


        public Property properties { get; private set; }

        public void AddProperty(Property property){ properties |= property;}

        public Type type { get; }

                     

        public enum Type
        {
            None = 0,

            Damage = 1,
            ChargeGauge = 2,
            Defence = 3,

            ������������� = 4,   //���س� ȸ���� �ƴ� ������� N���� ����ϴ� ���� ȿ��.
                           //���س� ȸ�������� �̺�Ʈ�� �߻���Ű�� �ʴ´�.
            ���������� = 5,   //�� ȹ���̳� �Ҹ𰡾ƴ�.
                           //��ǥ������ �� ���۽� �� �ʱ�ȭ�ɶ� ���.

            ������Ÿ�� =6,
            �߾���Ÿ�� =7,

            ����ġ = 10,

            ȿ���ο� = 11,
            ȿ��ȸ�� = 12,
            ȿ������ = 13,

            �Ҹ�ǰȹ�� = 20,

            ������ġ���� = 100,

            LengthMag = 65, // �̵��� �Ÿ������� ����
            PlayerSize = 66, // 

        }
        public enum Property
        {
            None = 0,
            ������ = 1, //���ط��϶��� ���
            ������ġ = 2,   //�ٸ�ȿ���κ��� �����Ǵ� ������ ��������
                        //����Ƽ �����ÿ�, ���������� ��ġ�� �׻� ���� (�������� ���ܻ�Ȳ�� �������� 0���ιٲ�)
            ũ��Ƽ�� = 4,
            �ݰݹ��� = 8,

            //->�����÷���
            ���ε����� = 16,


            ȿ���鿪�� = 32, //�鿪 �ؽ�Ʈ ���� ����

            �������ü = 64, //����� ȿ�� ���� ����

            ������������ = 128, //��ũ�ι�ƽ���� ���� ���� �ʴ� ���� ���� ������ ��� ���
        }

    

    }
}
