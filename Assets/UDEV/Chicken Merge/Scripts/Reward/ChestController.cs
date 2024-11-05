using UDEV.DMTool;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class ChestController : Singleton<ChestController>
    {
        public ChestUserData CurrentChest
        {
            get
            {
                var chests = UserDataHandler.Ins.chests;
                if (chests == null || chests.Count <= 0) return null;
                int chestId = chests.Count - 1;
                return chests[chestId];
            }
        }

        public int ChestRemaining
        {
            get
            {
                var chests = UserDataHandler.Ins.chests;
                if (chests == null || chests.Count <= 0) return 0;
                return chests.Count;
            }
        }

        public void OpenChest(UnityAction OnOpenChest = null)
        {
            if (CurrentChest == null) return;
            RewardDialog rewardDialog = (RewardDialog)DialogDB.Ins.GetDialog(DialogType.Reward);
            if(rewardDialog == null) return;
            rewardDialog.AddCoinRewardItem(CurrentChest.coinsBonus);
            var skillBonusList = CurrentChest.skillBonusList;
            if (skillBonusList == null || skillBonusList.Count <= 0) return;
            foreach ( var skillBonus in skillBonusList )
            {
                if(skillBonus == null ) continue;
                SkillCollectedManager.Ins.AddSkill(skillBonus.type, skillBonus.bonusAmount);
                var skillCtr = SkillCollectedManager.Ins.GetSkillController(skillBonus.type);
                if (skillCtr == null) continue;
                RewardDialogItem rewardDialogItem = new RewardDialogItem(skillCtr.skillStat.skillIcon, skillBonus.bonusAmount);
                rewardDialog.AddRewardItem(rewardDialogItem);
            }

            UserDataHandler.Ins.coin += CurrentChest.coinsBonus;
            UserDataHandler.Ins.chests.Remove(CurrentChest);
            UserDataHandler.Ins.SaveData();

            if(CurrentChest == null)
            {
                DialogDB.Ins.Show(rewardDialog);
            }else
            {
                DialogDB.Ins.Show(rewardDialog, ShowType.STACK);
            }

            DataGroup.Ins.UpdateQuests(QuestType.OpenChest, 1);
            OnOpenChest?.Invoke();
        }
    }
}
