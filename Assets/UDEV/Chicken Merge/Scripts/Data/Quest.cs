namespace UDEV.ChickenMerge
{
    [System.Serializable]
    public class Quest
    {
        private int id;
        public QuestType type;
        public int amountRequired;
        public int bonus;

        public int Id { get => id; set => id = value; }

        public bool IsCompleted(int id)
        {
            var questUserData = UserDataHandler.Ins.GetQuestData(id);
            if (questUserData == null) return false;
            return questUserData.amount >= amountRequired;
        }

        public bool IsClaimed(int id)
        {
            var questData = UserDataHandler.Ins.GetQuestData(id);
            if (questData == null) return false;
            return questData.isClaimed;
        }

        public string GetQuestDesctiption(QuestType type)
        {
            switch(type)
            {
                case QuestType.LevelUnlock:
                    return $"Unlock {amountRequired} Levels";
                case QuestType.WarriorUnlock:
                    return $"Unlock {amountRequired} Warriors";
                case QuestType.EnemyKill:
                    return $"Kill {amountRequired} Enemies";
                case QuestType.UseSkill:
                    return $"Use {amountRequired} Skills";
                case QuestType.UseBooster:
                    return $"Use {amountRequired} Boosters";
                case QuestType.WavePassed:
                    return $"Pass {amountRequired} Waves";
                case QuestType.UpgradeShield:
                    return $"Upgrade Shield {amountRequired} Times";
                case QuestType.OpenChest:
                    return $"Open {amountRequired} Chests";
            }
            return "";
        }
    }
}
