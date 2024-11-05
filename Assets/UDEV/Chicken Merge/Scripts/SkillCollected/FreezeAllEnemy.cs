using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class FreezeAllEnemy : SkillController
    {
        private void OnEnable()
        {
            OnTriggerEnter.AddListener(PlayBeepSound);
        }

        private void OnDisable()
        {
            OnTriggerEnter.RemoveListener(PlayBeepSound);
        }

        private void PlayBeepSound()
        {
            StartCoroutine(PlayBeepSoundCo());
        }

        private IEnumerator PlayBeepSoundCo()
        {
            while (m_isTriggered)
            {
                if (skillStat != null && skillStat.triggerSoundFx != null)
                {
                    yield return new WaitForSeconds(1f);
                    AudioController.Ins.PlaySound(skillStat.triggerSoundFx);
                }
            }
            yield return null;
            StopAllCoroutines();
        }
    }
}
