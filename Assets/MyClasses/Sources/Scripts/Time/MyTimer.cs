/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyTimer (version 1.3)
 */

namespace MyClasses
{
    public class MyTimer
    {
        #region ----- Variable -----

        private float _totalTime;
        private float _curTime;
        private bool _isJustDone;

        #endregion

        #region ----- Property -----

        public float TotalTime
        {
            get { return _totalTime; }
        }

        #endregion

        #region ----- Constructor -----

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyTimer()
        {
            _totalTime = 0;
            _curTime = 0;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MyTimer(float second)
        {
            SetTime(second);
        }

        #endregion

        #region ----- Public Method -----

        /// <summary>
        /// Set the number of seconds.
        /// </summary>
        public void SetTime(float second)
        {
            _totalTime = second;
            _curTime = second;
            _isJustDone = false;
        }

        /// <summary>
        /// Record the time has passed.
        /// </summary>
        public void Update(float dt)
        {
            if (_curTime > 0)
            {
                _curTime -= dt;
                if (_curTime < 0)
                {
                    _curTime = 0;
                }
            }
        }

        /// <summary>
        /// Return the number of seconds.
        /// </summary>
        public float GetTargetTime()
        {
            return _totalTime;
        }

        /// <summary>
        /// Return value of the percentage.
        /// </summary>
        public float GetPercent()
        {
            if (_totalTime > 0)
            {
                return 1f - (_curTime / _totalTime);
            }

            return 1f;
        }

        /// <summary>
        /// Check the timer finished.
        /// </summary>
        public bool IsDone()
        {
            return _curTime <= 0;
        }

        /// <summary>
        /// Check the timer have just finished.
        /// </summary>
        public bool IsJustDone()
        {
            if (!_isJustDone && IsDone())
            {
                _isJustDone = true;
                return true;
            }

            return false;
        }

        #endregion
    }
}