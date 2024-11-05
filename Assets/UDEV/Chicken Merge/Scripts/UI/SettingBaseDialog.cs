using UnityEngine;
using UnityEngine.UI;
using UDEV.DMTool;

namespace UDEV.ChickenMerge
{
    public class SettingBaseDialog : Dialog
    {
        [SerializeField] private Slider m_musicSlider;
        [SerializeField] private Slider m_soundSlider;

        public override void Show()
        {
            base.Show();

            if (m_musicSlider)
            {
                m_musicSlider.value = UserDataHandler.Ins.setting.musicVol;
                AudioBase.Ins.SetMusicVolume(UserDataHandler.Ins.setting.musicVol);
            }

            if (m_soundSlider)
            {
                m_soundSlider.value = UserDataHandler.Ins.setting.soundVol;
                AudioBase.Ins.SetSoundVolume(UserDataHandler.Ins.setting.soundVol);
            }
        }

        public void OnMusicChange(float value)
        {
            AudioBase.Ins.SetMusicVolume(value);
        }

        public void OnSoundChange(float value)
        {
            AudioBase.Ins.SetSoundVolume(value);
        }

        public virtual void Save()
        {
            UserDataHandler.Ins.setting.musicVol = AudioBase.Ins.musicVolume;
            UserDataHandler.Ins.setting.soundVol = AudioBase.Ins.sfxVolume;
            UserDataHandler.Ins.SaveData();
        }

        public override void Close()
        {
            base.Close();
            Save();
        }
    }
}
