/*
* Copyright (c) 2016 Phạm Minh Hoàng
* Framework:   MyClasses
* Class:       MyReactionParameter (version 1.1)
*/

namespace MyClasses.CoReaction
{
    #region ----- Base Class -----

    public abstract class MyReactionParameter
    {
        #region ----- Variable -----

        protected EType _type;
        protected string _name;
        protected object _value;

        #endregion

        #region ----- Property -----

        public EType Type
        {
            get { return _type; }
        }

        public string Name
        {
            get { return _name; }
        }

        public object Value
        {
            get { return _value; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionParameter(string name, object defaultValue)
        {
            SetName(name);
            SetValue(defaultValue);
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set the name.
        /// </summary>
        public void SetName(string name)
        {
            _name = name != null ? name : string.Empty;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        public virtual void SetValue(object value)
        {
        }

        /// <summary>
        /// Check if the value does not change.
        /// </summary>
        public virtual bool IsEqual(object comparedValue)
        {
            return false;
        }

        /// <summary>
        /// Check if the condition is satisfied.
        /// </summary>
        public virtual bool IsPass(MyReactionCondition.EComparedType comparedType, object comparedValue)
        {
            return false;
        }

        #endregion

        #region ----- Enumeration -----

        public enum EType
        {
            Float,
            Int,
            Bool,
            Trigger
        }

        #endregion
    }

    #endregion

    #region ----- Subclass -----

    public sealed class MyReactionFloatParameter : MyReactionParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionFloatParameter(string name, object defaultValue) : base(name, defaultValue)
        {
            _type = EType.Float;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        public override void SetValue(object value)
        {
            _value = value != null && value is float ? (float)value : 0;
        }

        /// <summary>
        /// Check if the value does not change.
        /// </summary>
        public override bool IsEqual(object value)
        {
            if (value is float)
            {
                return UnityEngine.Mathf.Abs((float)_value - (float)value) <= float.Epsilon;
            }
            return false;
        }

        /// <summary>
        /// Check if the condition is satisfied.
        /// </summary>
        public override bool IsPass(MyReactionCondition.EComparedType comparedType, object comparedValue)
        {
            if (comparedValue is float)
            {
                switch (comparedType)
                {
                    case MyReactionCondition.EComparedType.FloatGreater:
                        {
                            return (float)_value > (float)comparedValue;
                        }
                    case MyReactionCondition.EComparedType.FloatLess:
                        {
                            return (float)_value < (float)comparedValue;
                        }
                }
            }
            return false;
        }
    }

    public sealed class MyReactionIntParameter : MyReactionParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionIntParameter(string name, object defaultValue) : base(name, defaultValue)
        {
            _type = EType.Int;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        public override void SetValue(object value)
        {
            _value = value != null && value is int ? (int)value : 0;
        }

        /// <summary>
        /// Check if the value does not change.
        /// </summary>
        public override bool IsEqual(object comparedValue)
        {
            if (comparedValue is int)
            {
                return (int)_value == (int)comparedValue;
            }
            return false;
        }

        /// <summary>
        /// Check if the condition is satisfied.
        /// </summary>
        public override bool IsPass(MyReactionCondition.EComparedType comparedType, object comparedValue)
        {
            if (comparedValue is int)
            {
                switch (comparedType)
                {
                    case MyReactionCondition.EComparedType.IntEquals:
                        {
                            return (int)_value == (int)comparedValue;
                        }
                    case MyReactionCondition.EComparedType.IntNotEqual:
                        {
                            return (int)_value != (int)comparedValue;
                        }
                    case MyReactionCondition.EComparedType.IntGreater:
                        {
                            return (int)_value > (int)comparedValue;
                        }
                    case MyReactionCondition.EComparedType.IntLess:
                        {
                            return (int)_value < (int)comparedValue;
                        }
                }
            }

            return false;
        }
    }

    public sealed class MyReactionBoolParameter : MyReactionParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionBoolParameter(string name, object defaultValue) : base(name, defaultValue)
        {
            _type = EType.Bool;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        public override void SetValue(object value)
        {
            _value = value != null && value is bool ? (bool)value : false;
        }

        /// <summary>
        /// Check if the value does not change.
        /// </summary>
        public override bool IsEqual(object value)
        {
            if (value is bool)
            {
                return (bool)_value == (bool)value;
            }
            return false;
        }

        /// <summary>
        /// Check if the condition is satisfied.
        /// </summary>
        public override bool IsPass(MyReactionCondition.EComparedType comparedType, object comparedValue)
        {
            switch (comparedType)
            {
                case MyReactionCondition.EComparedType.BoolTrue:
                    {
                        return (bool)_value == true;
                    }
                case MyReactionCondition.EComparedType.BoolFalse:
                    {
                        return (bool)_value == false;
                    }
            }
            return false;
        }
    }

    public sealed class MyReactionTriggerParameter : MyReactionParameter
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MyReactionTriggerParameter(string name, object defaultValue) : base(name, defaultValue)
        {
            _type = EType.Trigger;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        public override void SetValue(object value)
        {
            _value = value != null && value is bool ? (bool)value : false;
        }

        /// <summary>
        /// Check if the value does not change.
        /// </summary>
        public override bool IsEqual(object comparedValue)
        {
            if (comparedValue is bool)
            {
                return (bool)_value == (bool)comparedValue;
            }
            return false;
        }

        /// <summary>
        /// Check if the condition is satisfied.
        /// </summary>
        public override bool IsPass(MyReactionCondition.EComparedType comparedType, object comparedValue)
        {
            switch (comparedType)
            {
                case MyReactionCondition.EComparedType.Trigger:
                    {
                        return (bool)_value;
                    }
            }
            return false;
        }
    }

    #endregion
}