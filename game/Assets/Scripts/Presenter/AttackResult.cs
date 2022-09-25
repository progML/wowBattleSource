using System;
using System.Text;

namespace Presenter
{
    [Serializable]
    public class AttackResult
    {
        public bool IsAttackSuccess;
        public UnitDeathType UnitDeathType;

        public AttackResult(bool isAttackSuccess, UnitDeathType unitDeathType)
        {
            IsAttackSuccess = isAttackSuccess;
            this.UnitDeathType = unitDeathType;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("isAttackSuccess :" + IsAttackSuccess);
            if (!IsAttackSuccess)
            {
                sb.Append("\n");
                sb.Append("UnitDeathType :" + UnitDeathType.name);
            }

            return sb.ToString();
        }
    }
}