using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UDEV
{
    [CustomPropertyDrawer(typeof(AnimatorStatesAttribute), true)]
    public class AnimatorStatesDrawer : PropertyDrawer
    {
        List<string> animatorStates;
        List<string> clips;
        List<int> layerIndexs;
        int selectedIndex;
        Animator curAnim;
        SerializedProperty stateProp, clipProp, layerIndexProp;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            animatorStates = new List<string>();
            clips = new List<string>();
            layerIndexs = new List<int>();
            animatorStates.Add("None");
            clips.Add("");
            layerIndexs.Add(0);

            Behaviour beh = property.serializedObject.targetObject as Behaviour;

            stateProp = property.FindPropertyRelative("name");

            clipProp = property.FindPropertyRelative("clipName");

            layerIndexProp = property.FindPropertyRelative("layerIndex");

            if (beh)
            {
                var rootObj = beh.transform.root;

                if (rootObj != null)
                {
                    curAnim = GetAnimator(rootObj);
                }
                else
                {
                    curAnim = GetAnimator(beh.transform);
                }

                if (curAnim != null && curAnim.runtimeAnimatorController != null)
                {
                    UnityEditor.Animations.AnimatorController baseController = curAnim.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;

                    if (baseController)
                    {
                        if(baseController.layers.Length > 0)
                        {
                            for (int i = 0; i < baseController.layers.Length; i++)
                            {
                                UnityEditor.Animations.AnimatorStateMachine sm = baseController.layers[i].stateMachine;

                                for (int j = 0; j < sm.states.Length; j++)
                                {
                                    animatorStates.Add(sm.states[j].state.name);
                                    var clip = (AnimationClip)sm.states[j].state.motion;

                                    if (clip)
                                        clips.Add(clip.name);
                                    else
                                        clips.Add("");

                                    layerIndexs.Add(i);
                                }
                            }
                        }
                    }

                    if (animatorStates != null && animatorStates.Count > 0)
                    {
                        if (stateProp != null && clipProp != null)
                        {
                            selectedIndex = Array.IndexOf(animatorStates.ToArray(), stateProp.stringValue);

                            selectedIndex = selectedIndex < 0 ? 0 : selectedIndex;

                            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, animatorStates.ToArray());

                            stateProp.stringValue = animatorStates[selectedIndex];

                            clipProp.stringValue = clips[selectedIndex];

                            layerIndexProp.intValue = layerIndexs[selectedIndex];
                        }
                    }
                }
            }

            if (curAnim == null ||
                curAnim.runtimeAnimatorController == null ||
                animatorStates == null || animatorStates.Count <= 0 || beh == null)
            {
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(position, property, label);
                if (stateProp != null)
                    stateProp.stringValue = "None";
                EditorGUI.EndProperty();
            }

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        Animator GetAnimator(Transform root)
        {
            if (root)
            {
                var rootAnim = root.GetComponent<Animator>();

                if (rootAnim) return rootAnim;

                var maxChild = root.childCount;
                for (int i = 0; i < maxChild; i++)
                {
                    var child = root.GetChild(i);

                    if (child)
                    {
                        var anim = child.GetComponent<Animator>();

                        if (anim) return anim;
                    }
                }
            }

            return null;
        }
    }
}
