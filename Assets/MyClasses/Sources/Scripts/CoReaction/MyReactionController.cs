/*
* Copyright (c) 2016 Phạm Minh Hoàng
* Framework:   MyClasses
* Class:       MyReactionController (version 1.1)
*/

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace MyClasses.CoReaction
{
    public class MyReactionController : MonoBehaviour
    {
        #region ----- Variable -----

        private Dictionary<int, MyReactionParameter> _dictionaryParameter = new Dictionary<int, MyReactionParameter>();
        private List<MyReaction> _listReaction = new List<MyReaction>();
        private int _countParameter;
        private int _countReaction;
        private bool _isHasParameterChange;
        private bool _isHasTriggerParameterChange;

        #endregion

        #region ----- Property -----

        public Dictionary<int, MyReactionParameter> DictionaryParameter
        {
            get { return _dictionaryParameter; }
        }

        #endregion

        #region ----- MonoBehaviour Implementation -----

        /// <summary>
        /// LateUpdate.
        /// </summary>
        void LateUpdate()
        {
            if (_isHasParameterChange)
            {
                _CheckConditionAndActiveReactions();
                _isHasParameterChange = false;
            }

            _RunReactions();

            if (_isHasTriggerParameterChange)
            {
                _ResetAllTriggerPamameters();
                _isHasTriggerParameterChange = false;
                _isHasParameterChange = true;
            }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Add a parameter.
        /// </summary>
        public void AddParameter(int id, MyReactionParameter.EType type, string name = null, object defaultValue = null)
        {
            if (_dictionaryParameter.ContainsKey(id))
            {
                Debug.LogWarning("[" + typeof(MyReactionController).Name + "] AddParameter(): Duplicate parameterID=" + id + ".");
                return;
            }

            MyReactionParameter parameter = null;
            switch (type)
            {
                case MyReactionParameter.EType.Float:
                    {
                        parameter = new MyReactionFloatParameter(name, defaultValue);
                    }
                    break;
                case MyReactionParameter.EType.Int:
                    {
                        parameter = new MyReactionIntParameter(name, defaultValue);
                    }
                    break;
                case MyReactionParameter.EType.Bool:
                    {
                        parameter = new MyReactionBoolParameter(name, defaultValue);
                    }
                    break;
                case MyReactionParameter.EType.Trigger:
                    {
                        parameter = new MyReactionTriggerParameter(name, defaultValue);
                    }
                    break;
            }

            _dictionaryParameter.Add(id, parameter);
            _countParameter = _dictionaryParameter.Count;
        }

        /// <summary>
        /// Add a reaction.
        /// </summary>
        public void AddReaction(MyReaction reaction)
        {
            if (_countParameter == 0)
            {
                Debug.LogWarning("[" + typeof(MyReactionController).Name + "] AddReaction(): You must setup parameters first.");
                return;
            }

            if (reaction == null)
            {
                Debug.LogWarning("[" + typeof(MyReactionController).Name + "] AddReaction(): Null reaction.");
                return;
            }

            foreach (MyReactionCondition condition in reaction.ListCondition)
            {
                if (!_dictionaryParameter.ContainsKey(condition.ParameterID))
                {
                    Debug.LogWarning("[" + typeof(MyReactionController).Name + "] AddReaction(): Could not find parameterID=" + condition.ParameterID);
                    continue;
                }
                condition.SetComparedFunction(_dictionaryParameter[condition.ParameterID].IsPass);
            }

            _listReaction.Add(reaction);
            _countReaction = _listReaction.Count;
        }

        /// <summary>
        /// Set a float value.
        /// </summary>
        public void SetFloat(int parameterID, float newValue)
        {
            _SetParameterValue(parameterID, newValue);
        }

        /// <summary>
        /// Set a int value.
        /// </summary>
        public void SetInt(int parameterID, int newValue)
        {
            _SetParameterValue(parameterID, newValue);
        }

        /// <summary>
        /// Set a bool value.
        /// </summary>
        public void SetBool(int parameterID, bool newValue)
        {
            _SetParameterValue(parameterID, newValue);
        }

        /// <summary>
        /// Active a trigger.
        /// </summary>
        public void SetTrigger(int parameterID)
        {
            _SetParameterValue(parameterID, true);
        }

        #endregion

        #region ----- Private Method -----

        /// <summary>
        /// Set the value of a parameter.
        /// </summary>
        private void _SetParameterValue(int parameterID, object value)
        {
            if (!_dictionaryParameter.ContainsKey(parameterID))
            {
                Debug.LogWarning("[" + typeof(MyReactionController).Name + "] _SetParameterValue(): Could not find parameterID=" + parameterID);
                return;
            }

            var parameter = _dictionaryParameter[parameterID];
            if (parameter.IsEqual(value))
            {
                return;
            }

            parameter.SetValue(value);

            _isHasParameterChange = true;
            if (parameter.Type == MyReactionParameter.EType.Trigger)
            {
                _isHasTriggerParameterChange = true;
            }
        }

        /// <summary>
        /// Run all reactions.
        /// </summary>
        private void _RunReactions()
        {
            for (int i = 0; i < _countReaction; i++)
            {
                _listReaction[i].Run();
            }
        }

        /// <summary>
        /// Active reactions if its conditions are satisfied.
        /// </summary>
        private void _CheckConditionAndActiveReactions()
        {
            for (int i = 0; i < _countReaction; i++)
            {
                _listReaction[i].CheckConditionsAndActive();
            }
        }

        /// <summary>
        /// Reset all trigger pamameters.
        /// </summary>
        private void _ResetAllTriggerPamameters()
        {
            foreach (var item in _dictionaryParameter)
            {
                var parameter = item.Value;
                if (parameter.Type == MyReactionParameter.EType.Trigger)
                {
                    parameter.SetValue(false);
                }
            }
        }

        #endregion
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(MyReactionController))]
    public class MyReactionControllerEditor : Editor
    {
        private MyReactionController _script;

        /// <summary>
        /// OnEnable.
        /// </summary>
        void OnEnable()
        {
            _script = (MyReactionController)target;
        }

        /// <summary>
        /// OnInspectorGUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(_script), typeof(MyReactionController), false);

            foreach (var parameter in _script.DictionaryParameter)
            {
                GUILayout.Label(parameter.Value.Name + ": " + parameter.Value.Value);
            }
        }
    }

#endif
}