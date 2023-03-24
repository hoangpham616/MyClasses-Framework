/*
* Copyright (c) 2016 Phạm Minh Hoàng
* Framework:   MyClasses
* Class:       MyReactionCondition (version 1.1)
*/

using System;

namespace MyClasses.CoReaction
{
    public class MyReactionCondition
    {
        #region ----- Variable -----

        private int _parameterID;
        private EComparedType _comparedType;
        private object _comparedValue;
        private Func<MyReactionCondition.EComparedType, object, bool> _comparedFunction;

        #endregion

        #region ----- Property -----

        public int ParameterID
        {
            get { return _parameterID; }
        }

        public EComparedType ComparedType
        {
            get { return _comparedType; }
        }

        public object ComparedValue
        {
            get { return _comparedValue; }
        }

        public Func<MyReactionCondition.EComparedType, object, bool> ComparedFunction
        {
            get { return _comparedFunction; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionCondition(int parameterID, EComparedType comparedType, object comparedValue)
        {
            _parameterID = parameterID;
            _comparedType = comparedType;
            _comparedValue = comparedValue;
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set the compared function.
        /// </summary>
        public void SetComparedFunction(Func<MyReactionCondition.EComparedType, object, bool> comparedFunction)
        {
            _comparedFunction = comparedFunction;
        }

        #endregion

        #region ----- Enumeration -----

        public enum EComparedType
        {
            FloatGreater,
            FloatLess,
            IntGreater,
            IntLess,
            IntEquals,
            IntNotEqual,
            BoolTrue,
            BoolFalse,
            Trigger,
        }

        #endregion
    }
}