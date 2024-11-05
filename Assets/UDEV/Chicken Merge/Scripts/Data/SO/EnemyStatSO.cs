using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    //[CreateAssetMenu(fileName = "New Enemy Data", menuName = "UDEV/Create Enemy Data")]
    public class EnemyStatSO : Stat
    {
        [Header("Common:")]
        public float hp;
        public float damage;
        [Range(0f, 1f)]
        public float damageMissRange = 0.5f;
        public float moveSpeed;
        [Range(0f, 1f)]
        public float attackSpeed = 0.8f;
        [Range(0f, 1f)]
        public float minAttackSpeed;
        public int editorMaxLevel;
        public int editorLevel;
        [Header("Bonus:")]
        [Range(1, 50)]
        public int coinBonusMultier = 1;

        [Header("Level Up:")]
        public float hpUpRate;
        public float damageUp;
        public float attackSpeedUp;

        [Header("Level Up Factor:")]
        public float hpUpFactor = 2;
        public float damageUpFactor = 2;
        public float attackSpeedUpFactor = 25;

        private float m_staringHp;
        private float m_staringAttackSpeed;
        private float m_staringDamage;

        public float RealHp
        {
            get => Helper.MaxUpgradeValue(hpUpFactor, hp, hpUpRate, UserDataHandler.Ins.curLevelId + 1);
        }

        public float RealDamage
        {
            get
            {
                var finalDamage = Helper.MaxUpgradeValue(damageUpFactor, damage, damageUp, UserDataHandler.Ins.curLevelId + 1);
                var damageMissed = finalDamage * Random.Range(0, damageMissRange);
                return finalDamage - damageMissed;
            }
        }

        public float RealAttackSpeed
        {
            get {
                float finalAtkSpeed = Helper.MaxUpgradeValue(attackSpeedUpFactor, attackSpeed, attackSpeedUp, UserDataHandler.Ins.curLevelId + 1);
                finalAtkSpeed = Mathf.Clamp(finalAtkSpeed, minAttackSpeed, attackSpeed);
                return finalAtkSpeed;
            }
        }

        public float RealHpEditor
        {
            get => Helper.MaxUpgradeValue(hpUpFactor, hp, hpUpRate, editorLevel);
        }

        public float RealDamageEditor
        {
            get => Helper.MaxUpgradeValue(damageUpFactor, damage, damageUp, editorLevel);
        }

        public float RealAttackSpeedEditor
        {
            get => Helper.MaxUpgradeValue(attackSpeedUpFactor, attackSpeed, attackSpeedUp, editorLevel);
        }

        public override bool IsMaxLevel()
        {
            return editorLevel >= editorMaxLevel;
        }

        public override void Upgrade(UnityAction Success = null, UnityAction Failed = null)
        {
            if(editorLevel <= 0)
            {
                TrackingStats();
            }
            UpgradeCore();
        }

        protected override void UpgradeCore()
        {
            LoadOrigianlStats();
            editorLevel++;
            hp = RealHpEditor;
            damage = RealDamageEditor;
            attackSpeed = RealAttackSpeedEditor;
            attackSpeed = Mathf.Clamp(attackSpeed, minAttackSpeed, attackSpeed);
        }

        public override void UpgradeToMax(UnityAction OnUpgrade = null)
        {
            TrackingStats();
            while (!IsMaxLevel())
            {
                LoadOrigianlStats();
                UpgradeCore();
                OnUpgrade?.Invoke();
            }
        }

        private void TrackingStats()
        {
            m_staringHp = hp;
            m_staringAttackSpeed = attackSpeed;
            m_staringDamage = damage;
        }

        private void LoadOrigianlStats()
        {
            hp = m_staringHp;
            attackSpeed = m_staringAttackSpeed;
            damage = m_staringDamage;
        }

        public override string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
