using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_WaveCtrSettings_Layout : WaveTK_LayoutBuilder
    {
        private SerializedObject m_waveCtrSerializedObj;
        private SerializedProperty m_spawnAtTransformProp;
        private SerializedProperty m_spawnPostionProp;
        private SerializedProperty m_spawnTransformsProp;
        private SerializedProperty m_randomSpawnPositionProp;

        private PropertyField m_spawnAtTransformPropUI;
        private PropertyField m_spawnPositionPropUI;
        private PropertyField m_spawnTransformsPropUI;
        private PropertyField m_randomSpawnPositionPropUI;


        public WaveTK_WaveCtrSettings_Layout(string templateName, WaveTK_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        public override void GetUIProperties()
        {
            m_spawnAtTransformPropUI = m_container.Q<PropertyField>("SpawnAtTransform");
            m_spawnPositionPropUI = m_container.Q<PropertyField>("SpawnPosition");
            m_spawnTransformsPropUI = m_container.Q<PropertyField>("SpawnTransforms");
            m_randomSpawnPositionPropUI = m_container.Q<PropertyField>("RandomSpawnPosition");
        }

        public void SetWaveCtrSerializedObj(SerializedObject serializedObject)
        {
            m_waveCtrSerializedObj = serializedObject;
            m_spawnAtTransformProp = m_waveCtrSerializedObj.FindProperty("spawnAtTransform");
            m_spawnPostionProp = m_waveCtrSerializedObj.FindProperty("spawnPosition");
            m_spawnTransformsProp = m_waveCtrSerializedObj.FindProperty("spawnTransforms");
            m_randomSpawnPositionProp = m_waveCtrSerializedObj.FindProperty("randomSpawnPos");
        }

        public override void SetProperty(SerializedProperty property)
        {
            

        }

        protected override void BuildLayout()
        {
            m_spawnAtTransformPropUI.BindProperty(m_spawnAtTransformProp);
            m_spawnPositionPropUI.BindProperty(m_spawnPostionProp);
            m_spawnTransformsPropUI.BindProperty(m_spawnTransformsProp);
            m_randomSpawnPositionPropUI.BindProperty(m_randomSpawnPositionProp);

            m_spawnAtTransformPropUI.RegisterValueChangeCallback((prop) =>
            {
                WaveTK_UI_Utils.ShowElement(m_spawnPositionPropUI, !prop.changedProperty.boolValue);
                WaveTK_UI_Utils.ShowElement(m_spawnTransformsPropUI, prop.changedProperty.boolValue);
            });
        }
    }
}
