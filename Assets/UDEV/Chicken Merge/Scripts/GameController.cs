using System;
using System.Collections;
using UDEV.ActionEventDispatcher;
using UnityEngine;
using UDEV.WaveManagerToolkit;

namespace UDEV.ChickenMerge
{
    public class GameController : Singleton<GameController>, IActionEventDispatcher
    {
        public static GameState state;
        [SerializeField] private Transform[] m_spawnPoints;
        private WaveTK_WaveController m_wavePrefab;
        private WaveTK_WaveController m_curWave;
        [SerializeField] private Shield m_curShield;

        private static GameState m_prevState;
        private int m_levelCoinBonus;

        #region ACTION
        private Action<object> m_OnGameover;
        private Action<object> m_OnEnemyDie;
        #endregion

        public int LevelCoinBonus { get => m_levelCoinBonus; }

        #region GAME_STATE
        public static bool IsPlaying
        {
            get => state == GameState.Playing;
        }

        public static bool IsGameover
        {
            get => state == GameState.Gameover;
        }

        public static bool IsCompleted
        {
            get => state == GameState.Completed;
        }
        #endregion

        public static bool IsPausing
        {
            get => state == GameState.Pausing;
        }
        public WaveTK_WaveController CurWave { get => m_curWave; }
        public Shield CurShield { get => m_curShield; }

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnGameover = param => LevelFailed();
            m_OnEnemyDie = param => m_curWave.AddEnemyKilled(1);

            AudioController.Ins?.RegisterEvents();

            this.RegisterActionEvent(EnemyAction.DIE, m_OnEnemyDie);
            this.RegisterActionEvent(GameState.Gameover, m_OnGameover);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(EnemyAction.DIE, m_OnEnemyDie);
            this.RemoveActionEvent(GameState.Gameover, m_OnGameover);
        }
        #endregion;

        protected override void Awake()
        {
            MakeSingleton(false);
            CreateAndSettingWave();
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void Start()
        {
            Init();

        #if UNITY_EDITOR
            StartCoroutine(SaveData());
        #endif
        }

        private void Update()
        {
            SwitchWaveState();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void Init()
        {
            m_levelCoinBonus = DataGroup.Ins.LevelBonus;
            m_curShield?.Init();

            ChangeState(GameState.Playing);

            Invoke("WaveBegin", 1f);

            this.PostActionEvent(GameState.Starting, m_wavePrefab);
            Pref.SpriteOrder = 0;
        }

        private void CreateAndSettingWave()
        {
            m_wavePrefab = DataGroup.Ins.CurWavePrefab;

            if (m_wavePrefab == null) return;

            m_curWave = Instantiate(m_wavePrefab, Vector3.zero, Quaternion.identity);
            m_curWave.spawnAtTransform = true;
            m_curWave.spawnTransforms.Clear();  
            m_curWave.spawnTransforms.AddRange(m_spawnPoints);
            m_curWave.OnWaveStarts.AddListener(() =>
            {
                this.PostActionEvent(GameplayAction.WAVE_BEGIN, m_curWave);
            });

            m_curWave.OnFinalWaveFinished.AddListener(() =>
            {
                InvokeRepeating("LevelCompleted", 1.5f, 0.5f);
            });
            m_curWave.OnWaveHasFinished.AddListener(() =>
            {
                this.PostActionEvent(GameplayAction.WAVE_PASSED, m_curWave);
            });

            m_curWave.OnEnemySpawned.AddListener(SpawnEnemy);
        }

        private void WaveBegin()
        {
            m_curWave?.StartWave();
        }

        private void SwitchWaveState()
        {
            if (m_curWave == null) return;

            if(state != GameState.Playing)
            {
                m_curWave.PauseWave();
            }else
            {
                m_curWave.ResumeWave();
            }
        }

        private void SpawnEnemy(GameObject enemySpawned)
        {
            if (enemySpawned == null) return;

            var actorComp = enemySpawned.GetComponent<Actor>();
            var enemyVisual = enemySpawned.GetComponent<EnemyVisual>();
            enemySpawned.transform.position += actorComp.spawnOffset;
            actorComp?.Init();
            enemyVisual?.SpawnAppearVfx();
        }

        public void Replay()
        {
            m_curShield.Init();
            m_curWave?.ResetCurrentWave();
            m_curWave?.StartWave();
            ChangeState(GameState.Playing);

            this.PostActionEvent(GameState.Starting, m_wavePrefab);
            Pref.SpriteOrder = 0;
        }

        public void ClaimLevelBonus(int multier = 1)
        {
            UserDataHandler.Ins.coin += m_levelCoinBonus * multier;
            UserDataHandler.Ins?.SaveData();
        }

        private void LevelCompleted()
        {
            if (IsPausing || IsCompleted) return;
            state = GameState.Pausing;
            UnlockNextLevel();
            this.PostActionEvent(GameState.Completed);
        }

        private void LevelFailed()
        {
            state = GameState.Gameover;
        }

        private void UnlockNextLevel()
        {
            int nextLevelId = ++UserDataHandler.Ins.curLevelId;
            UserDataHandler.Ins?.UpdateLevelUnlocked(nextLevelId, true);
            UserDataHandler.Ins?.SaveData();

            //Lưu level tại đây
            Debug.Log("nextLevelId: " + nextLevelId);
            PlayerPrefs.SetInt("NextLevelId", nextLevelId);
            PlayerPrefs.Save(); // Lưu lại để đảm bảo giá trị được lưu
        }

        public static void ChangeState(GameState newState)
        {
            m_prevState = state;
            state = newState;
        }

        public static void RevertState()
        {
            state = m_prevState;
        }

        private void OnApplicationQuit()
        {
            UserDataHandler.Ins?.SaveData();
        }

        private void OnApplicationPause(bool pause)
        {
            UserDataHandler.Ins?.SaveData();
        }

        private IEnumerator SaveData()
        {
            while (true)
            {
                yield return new WaitForSeconds(5);
                UserDataHandler.Ins?.SaveData();
            }
        }
    }
}
