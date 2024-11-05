using System.Collections;
using UnityEngine;
using MonsterLove.StateMachine;
using UDEV.ActionEventDispatcher;
using System;

namespace UDEV.ChickenMerge
{
    public class ChickenController : MonoBehaviour, IActionEventDispatcher
    {
        [SerializeField]
        private GunController m_gunController;
        private Animator m_anim;
        private StateMachine<ChickenState> m_fsm;
        private Node m_curNode;

        #region ACTION
        private Action<object> m_OnLoadingGunStat;
        #endregion

        public Node CurNode { get => m_curNode; set => m_curNode = value; }
        public GunController GunController { get => m_gunController;}

        public Sprite ChickenPreview { get => m_gunController.Stat.thumb; }

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnLoadingGunStat = param => LoadStats((int)param);

            this.RegisterActionEvent(GameplayAction.UPGRADE_GUN, m_OnLoadingGunStat);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.UPGRADE_GUN, m_OnLoadingGunStat);
        }
        #endregion

        private void Awake()
        {
            m_anim = GetComponent<Animator>();
            m_fsm = StateMachine<ChickenState>.Initialize(this);
        }

        private void OnEnable()
        {
            RegisterEvents();

            m_fsm.ChangeState(ChickenState.Idle);
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        public void Init(int gunId)
        {
            m_gunController?.Init(gunId);
        }

        private void GunTrigger()
        {
            if (m_curNode == null || m_curNode.nodeType == NodeType.Static
                || !GameController.IsPlaying || m_gunController == null) return;

            m_gunController?.ShootTrigger();
        }

        private void LoadStats(int gunId)
        {
            m_gunController?.LoadStats(gunId);
        }

        public void StopShoot()
        {
            m_gunController?.StopShoot();
            m_fsm.ChangeState(ChickenState.Idle);
        }

        public void BackToShoot()
        {
            GunTrigger();
        }

        #region FSM
        private void Idle_Enter() {
            
        }
        private void Idle_Update() {
            GunTrigger();
            if(m_gunController && m_gunController.IsShooted && m_gunController.IsShooting)
            {
                m_fsm.ChangeState(ChickenState.Shoot);
            }
            Helper.PlayAnim(m_anim, ChickenState.Idle.ToString());
        }
        private void Idle_FixedUpdate() { }
        private void Idle_Exit() { }
        private void Shoot_Enter() {
            m_gunController.ShootTrigger();
        }
        private void Shoot_Update() {
            Helper.PlayAnim(m_anim, ChickenState.Shoot.ToString());
            if(m_gunController && !m_gunController.IsShooting)
            {
                m_fsm.ChangeState(ChickenState.Idle);
            }
        }
        private void Shoot_FixedUpdate() { }
        private void Shoot_Exit() { }
        #endregion
    }
}
