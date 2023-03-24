/*
* Copyright (c) 2016 Phạm Minh Hoàng
* Framework:   MyClasses
* Class:       MyReaction (version 1.1)
*/

using System;
using System.Collections.Generic;

namespace MyClasses.CoReaction
{
    public class MyReaction
    {
        #region ----- Variable -----

        private List<MyReactionCondition> _listCondition = new List<MyReactionCondition>();
        private Action _action;
        private bool _isActive;

        #endregion

        #region ----- Property -----

        public List<MyReactionCondition> ListCondition
        {
            get { return _listCondition; }
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Add a condition.
        /// </summary>
        public virtual void AddCondition(int parameterID, MyReactionCondition.EComparedType conditionType, object comparedValue = null)
        {
            _listCondition.Add(new MyReactionCondition(parameterID, conditionType, comparedValue));
        }

        /// <summary>
        /// Remove a condition.
        /// </summary>
        public virtual void RemoveCondition(int parameterID)
        {
            foreach (MyReactionCondition condition in _listCondition)
            {
                if (condition.ParameterID == parameterID)
                {
                    _listCondition.Remove(condition);
                }
            }
        }

        /// <summary>
        /// Remove all conditions.
        /// </summary>
        public virtual void RemoveAllConditions()
        {
            _listCondition.Clear();
        }

        /// <summary>
        /// Set the action.
        /// </summary>
        public virtual void SetAction(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// Active/deactive reaction.
        /// </summary>
        public virtual void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        /// <summary>
        /// Action.
        /// </summary>
        public virtual void Run()
        {
            if (_isActive && _action != null)
            {
                _action();
            }
        }

        /// <summary>
        /// Active this reaction if its conditions are satisfied.
        /// </summary>
        public virtual void CheckConditionsAndActive()
        {
            bool isPass = true;

            foreach (MyReactionCondition condition in _listCondition)
            {
                if (!condition.ComparedFunction(condition.ComparedType, condition.ComparedValue))
                {
                    isPass = false;
                    break;
                }
            }

            SetActive(isPass);
        }

        #endregion
    }
}