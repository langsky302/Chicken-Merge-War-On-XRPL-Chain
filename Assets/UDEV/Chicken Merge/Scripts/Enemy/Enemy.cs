using MonsterLove.StateMachine;
using System.Collections;
using UDEV.ActionEventDispatcher;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class Enemy : Actor
    {
        private EnemyStatSO m_curStat;
        private StateMachine<EnemyState> m_fsm;
        private EnemyState m_prevState;
        private CollectableManager m_collectableMng;
        private EnemyBonusReceiver m_bonusReceiver;
        private EnemySkillAffected m_skillAffected;
        private ShieldDetection m_shieldDetection;
        private Coroutine m_changeStateRoutine;

        private float m_attackRate;
        private bool m_isAttacking;

        private bool IsDead
        {
            get => m_fsm.State == EnemyState.Dead || m_fsm.State == EnemyState.Dead;
        }
        public EnemyStatSO CurStat { get => m_curStat;}

        protected override void Awake()
        {
            base.Awake();
            m_collectableMng = GetComponent<CollectableManager>();
            m_bonusReceiver = GetComponent<EnemyBonusReceiver>();
            m_skillAffected = GetComponent<EnemySkillAffected>();
            m_shieldDetection = GetComponentInChildren<ShieldDetection>();

            m_fsm = StateMachine<EnemyState>.Initialize(this);
        }

        public override void Init(bool isBoss = false)
        {
            base.Init(isBoss);
            gameObject.layer = m_normalLayer;
            m_isAttacking = false;
            LoadStats();
            m_bonusReceiver?.UpdateSkillBoosterBonus();
            m_skillAffected?.SetupSkill();
            m_fsm.ChangeState(EnemyState.Walk);
        }

        protected override void LoadStats()
        {
            m_curStat = (EnemyStatSO)m_stat;

            if (m_curStat == null) return;

            m_curHp = m_curStat.RealHp;
            m_curMoveSpeed = m_curStat.moveSpeed;
            m_attackRate = m_curStat.RealAttackSpeed;

            OnLoadStat?.Invoke(m_curStat);
        }

        protected void UpdateMoveSpeed()
        {
            m_curMoveSpeed = GetRealMoveSpeed();
            m_curMoveSpeed += Random.Range(-0.1f, 0.1f);
            if (m_skillAffected && m_skillAffected.IsMovementReduced)
            {
                m_anim.speed = m_startingAnimSpeed / 2f;
            }
            else
            {
                m_anim.speed = m_startingAnimSpeed;
            }
        }

        private float GetRealMoveSpeed()
        {
            if (m_skillAffected == null) return m_curStat.moveSpeed;
            if(m_skillAffected.IsMovementReduced) return m_curStat.moveSpeed / 2;
            return m_curStat.moveSpeed;
        }

        public void ChangeState(EnemyState state)
        {
            if(m_fsm == null) return;
            m_prevState = m_fsm.State;
            m_fsm.ChangeState(state);
        }

        private IEnumerator ChangeStateDelayCo(EnemyState newState, float timeExtra = 0)
        {
            var animClip = Helper.GetClip(m_anim, m_fsm.State.ToString());
            if (animClip)
            {
                yield return new WaitForSeconds(animClip.length + timeExtra);
                m_fsm.ChangeState(newState);
            }

            yield return null;
        }

        private void ChangeStateDalay(EnemyState newState, float timeExtra = 0)
        {
            if (m_changeStateRoutine != null)
            {
                StopCoroutine(m_changeStateRoutine);
            }
            m_changeStateRoutine = StartCoroutine(ChangeStateDelayCo(newState, timeExtra));
        }

        private void AttackChecking()
        {
            if (m_skillAffected && m_skillAffected.IsFreezing) return;

            m_attackRate -= Time.deltaTime;
            if (m_shieldDetection == null || !m_shieldDetection.IsDetected || m_attackRate > 0) return;
            ChangeState(EnemyState.Attack);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            this.PostActionEvent(EnemyAction.TAKE_DAMAGE, this);
        }

        protected override void Dead()
        {
            ChangeState(EnemyState.Dead);

            int realCoinBonusMultier = 0;
            int coinBonusMultier = m_curStat.coinBonusMultier * (UserDataHandler.Ins.curLevelId + 1);
            realCoinBonusMultier = coinBonusMultier;
            if (m_bonusReceiver)
            {
                realCoinBonusMultier = m_bonusReceiver.GetBonusMultier(coinBonusMultier);
            }

            m_collectableMng?.SpawnCollectable(transform.position, realCoinBonusMultier);
            gameObject.layer = m_deadLayer;
            OnDead?.Invoke();
            this.PostActionEvent(EnemyAction.DIE);
            gameObject.SetActive(false);
        }

        public void DeadTrigger()
        {
            Dead();
        }

        #region FSM
        private void Idle_Enter() { }
        private void Idle_Update() {
            Helper.PlayAnim(m_anim, EnemyState.Idle.ToString());

            AttackChecking();

            if (GameController.state == GameState.Playing)
            {
                if (m_skillAffected == null || m_skillAffected.IsFreezing || m_isAttacking) return;
                ChangeState(EnemyState.Walk);
            }
        }
        private void Idle_Exit() { }
        private void Walk_Enter() {
        }
        private void Walk_Update() {
            UpdateMoveSpeed();
            Helper.PlayAnim(m_anim, EnemyState.Walk.ToString());
            AttackChecking();

            if((m_skillAffected && m_skillAffected.IsFreezing) || GameController.state != GameState.Playing)
            {
                ChangeState(EnemyState.Idle);
            }
        }

        private void Walk_FixedUpdate() {
            Move();
        }
        private void Walk_Exit() {
            StopMove();
        }

        private void Attack_Enter()
        {
            m_isAttacking = true;
            ChangeStateDalay(EnemyState.Idle);
        }
        private void Attack_Update()
        {
            Helper.PlayAnim(m_anim, EnemyState.Attack.ToString());
        }

        private void Attack_Exit()
        {
            m_attackRate = m_curStat.RealAttackSpeed + Random.Range(-0.05f, 0.05f);
        }

        private void Dead_Enter() {
            
        }
        private void Dead_Update() { }
        private void Dead_Exit() { }
        #endregion
    }
}
