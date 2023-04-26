/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Email:       hoangpham61691@gmail.com
 * Framework:   MyClasses
 * Class:       MyUtilities.Anchor (version 1.2)
 */

using UnityEngine;

namespace MyClasses
{
    public static partial class MyUtilities
    {
        #region ----- Public Function -----

        /// <summary>
        /// Check if Anchor Presets are stretch type.
        /// </summary>
        public static bool IsStretchAnchorPreset(EAnchorPreset anchorPreset)
        {
            switch (anchorPreset)
            {
                case EAnchorPreset.VerticalStretchLeft:
                case EAnchorPreset.VerticalStretchCenter:
                case EAnchorPreset.VerticalStretchRight:
                case EAnchorPreset.HorizontalStretchTop:
                case EAnchorPreset.HorizontalStretchMiddle:
                case EAnchorPreset.HorizontalStretchBottom:
                case EAnchorPreset.DualStretch:
                    {
                        return true;
                    }
            }

            return false;
        }

        /// <summary>
        /// Returns GameObject's Anchor Presets.
        /// </summary>
        public static EAnchorPreset GetAnchorPreset(GameObject gameObject)
        {
            return GetAnchorPreset(gameObject.GetComponent<RectTransform>());
        }

        /// <summary>
        /// Returns RectTransform's Anchor Presets.
        /// </summary>
        public static EAnchorPreset GetAnchorPreset(RectTransform rectTransform)
        {

            Vector2 zero_one = Vector2.zero;
            zero_one.y = 1;
            if (rectTransform.anchorMin == zero_one)
            {
                if (rectTransform.anchorMax == zero_one)
                {
                    return EAnchorPreset.TopLeft;
                }
                if (rectTransform.anchorMax == Vector2.one)
                {
                    return EAnchorPreset.HorizontalStretchTop;
                }
            }
            
            Vector2 half_one = Vector2.one;
            half_one.x = 0.5f;
            if (rectTransform.anchorMin == half_one)
            {
                if (rectTransform.anchorMax == half_one)
                {
                    return EAnchorPreset.TopCenter;
                }
            }

            if (rectTransform.anchorMin == Vector2.one)
            {
                if (rectTransform.anchorMax == Vector2.one)
                {
                    return EAnchorPreset.TopRight;
                }
            }

            Vector2 zero_half = Vector2.zero;
            zero_half.y = 0.5f;
            Vector2 one_half = Vector2.one;
            one_half.y = 0.5f;
            if (rectTransform.anchorMin == zero_half)
            {
                if (rectTransform.anchorMax == zero_half)
                {
                    return EAnchorPreset.MiddleLeft;
                }
                if (rectTransform.anchorMax == one_half)
                {
                    return EAnchorPreset.HorizontalStretchMiddle;
                }
            }

            Vector2 half_half = Vector2.one * 0.5f;
            if (rectTransform.anchorMin == half_half)
            {
                if (rectTransform.anchorMax == half_half)
                {
                    return EAnchorPreset.MiddleCenter;
                }
            }

            Vector2 one_zero = Vector2.one;
            one_zero.y = 0;
            if (rectTransform.anchorMin == one_half)
            {
                if (rectTransform.anchorMax == one_half)
                {
                    return EAnchorPreset.MiddleRight;
                }
                if (rectTransform.anchorMax == one_zero)
                {
                    return EAnchorPreset.BottomRight;
                }
            }

            if (rectTransform.anchorMin == Vector2.zero)
            {
                if (rectTransform.anchorMax == Vector2.zero)
                {
                    return EAnchorPreset.BottomLeft;
                }
                if (rectTransform.anchorMax == zero_one)
                {
                    return EAnchorPreset.VerticalStretchLeft;
                }
                if (rectTransform.anchorMax == one_zero)
                {
                    return EAnchorPreset.HorizontalStretchBottom;
                }
                if (rectTransform.anchorMax == Vector2.one)
                {
                    return EAnchorPreset.DualStretch;
                }
            }

            Vector2 half_zero = Vector2.zero;
            half_zero.x = 0.5f;
            if (rectTransform.anchorMin == half_zero)
            {
                if (rectTransform.anchorMax == half_zero)
                {
                    return EAnchorPreset.BottomCenter;
                }
                if (rectTransform.anchorMax == half_one)
                {
                    return EAnchorPreset.VerticalStretchCenter;
                }
            }

            if (rectTransform.anchorMin == one_zero)
            {
                if (rectTransform.anchorMax == Vector2.one)
                {
                    return EAnchorPreset.VerticalStretchRight;
                }
            }

            return EAnchorPreset.Unknown;
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        public static void Anchor(ref RectTransform rectTransform, EAnchorPreset anchorPreset)
        {
            _SetAnchorPreset(ref rectTransform, anchorPreset);
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        public static void Anchor(ref RectTransform rectTransform, EAnchorPivot anchorPivot)
        {
            _SetAnchorPivot(ref rectTransform, anchorPivot);
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        public static void Anchor(ref RectTransform rectTransform, EAnchorPreset anchorPreset, EAnchorPivot anchorPivot)
        {
            _SetAnchorPreset(ref rectTransform, anchorPreset);
            _SetAnchorPivot(ref rectTransform, anchorPivot);
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        /// <param name="offsetX">anchored position x</param>
        /// <param name="offsetY">anchored position y</param>
        public static void Anchor(ref RectTransform rectTransform, EAnchorPreset anchorPreset, EAnchorPivot anchorPivot, float offsetX, float offsetY)
        {
            _SetAnchorPreset(ref rectTransform, anchorPreset);
            _SetAnchorPivot(ref rectTransform, anchorPivot);

            rectTransform.anchoredPosition = new Vector2(offsetX, offsetY);
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        /// <param name="offsetX">anchored position x</param>
        /// <param name="offsetY">anchored position y</param>
        public static void Anchor(ref RectTransform rectTransform, EAnchorPreset anchorPreset, EAnchorPivot anchorPivot, float width, float height, float offsetX, float offsetY)
        {
            _SetAnchorPreset(ref rectTransform, anchorPreset);
            _SetAnchorPivot(ref rectTransform, anchorPivot);

            rectTransform.anchoredPosition = new Vector2(offsetX, offsetY);
            rectTransform.sizeDelta = new Vector2(width, height);
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        /// <param name="minOffset">the offset of the lower left corner of the rectangle relative to the lower left anchor</param>
        /// <param name="maxOffset">the offset of the upper right corner of the rectangle relative to the upper right anchor</param>
        public static void Anchor(ref RectTransform rectTransform, EAnchorPreset anchorPreset, EAnchorPivot anchorPivot, Vector2 minOffset, Vector2 maxOffset)
        {
            _SetAnchorPreset(ref rectTransform, anchorPreset);
            _SetAnchorPivot(ref rectTransform, anchorPivot);

            rectTransform.offsetMin = minOffset;
            rectTransform.offsetMax = maxOffset;
        }

        /// <summary>
        /// Anchor.
        /// </summary>
        /// <param name="minOffset">the offset of the lower left corner of the rectangle relative to the lower left anchor</param>
        /// <param name="maxOffset">the offset of the upper right corner of the rectangle relative to the upper right anchor</param>
        public static void Anchor(ref GameObject gameObject, EAnchorPreset anchorPreset, EAnchorPivot anchorPivot, Vector2 minOffset, Vector2 maxOffset)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform!= null)
            {
                Anchor(ref rectTransform, anchorPreset, anchorPivot, minOffset, maxOffset);
            }
        }

        #endregion

        #region ----- Private Function -----

        /// <summary>
        /// Set anchor preset.
        /// </summary>
        private static void _SetAnchorPreset(ref RectTransform rectTransform, EAnchorPreset anchorPreset)
        {
            switch (anchorPreset)
            {
                case EAnchorPreset.TopLeft:
                    {
                        Vector2 zero_one = Vector2.zero;
                        zero_one.y = 1;
                        rectTransform.anchorMin = zero_one;
                        rectTransform.anchorMax = zero_one;
                    }
                    break;

                case EAnchorPreset.TopCenter:
                    {
                        Vector2 half_one = Vector2.one;
                        half_one.x = 0.5f;
                        rectTransform.anchorMin = half_one;
                        rectTransform.anchorMax = half_one;
                    }
                    break;

                case EAnchorPreset.TopRight:
                    {
                        rectTransform.anchorMin = Vector2.one;
                        rectTransform.anchorMax = Vector2.one;
                    }
                    break;

                case EAnchorPreset.MiddleLeft:
                    {
                        Vector2 zero_half = Vector2.zero;
                        zero_half.y = 0.5f;
                        rectTransform.anchorMin = zero_half;
                        rectTransform.anchorMax = zero_half;
                    }
                    break;

                case EAnchorPreset.MiddleCenter:
                    {
                        Vector2 half = Vector2.one * 0.5f;
                        rectTransform.anchorMin = half;
                        rectTransform.anchorMax = half;
                    }
                    break;

                case EAnchorPreset.MiddleRight:
                    {
                        Vector2 one_half = Vector2.one;
                        one_half.y = 0.5f;
                        rectTransform.anchorMin = one_half;
                        rectTransform.anchorMax = one_half;
                    }
                    break;

                case EAnchorPreset.BottomLeft:
                    {
                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchorMax = Vector2.zero;
                    }
                    break;

                case EAnchorPreset.BottomCenter:
                    {
                        Vector2 half_zero = Vector2.zero;
                        half_zero.x = 0.5f;
                        rectTransform.anchorMin = half_zero;
                        rectTransform.anchorMax = half_zero;
                    }
                    break;

                case EAnchorPreset.BottomRight:
                    {
                        Vector2 one_zero = Vector2.one;
                        one_zero.y = 0;
                        rectTransform.anchorMin = one_zero;
                        rectTransform.anchorMax = one_zero;
                    }
                    break;

                case EAnchorPreset.VerticalStretchLeft:
                    {
                        Vector2 zero_one = Vector2.zero;
                        zero_one.y = 1;
                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchorMax = zero_one;
                    }
                    break;

                case EAnchorPreset.VerticalStretchCenter:
                    {
                        Vector2 half_zero = Vector2.zero;
                        half_zero.x = 0.5f;
                        Vector2 half_one = Vector2.one;
                        half_one.x = 0.5f;
                        rectTransform.anchorMin = half_zero;
                        rectTransform.anchorMax = half_one;
                    }
                    break;

                case EAnchorPreset.VerticalStretchRight:
                    {
                        Vector2 one_zero = Vector2.one;
                        one_zero.y = 0;
                        rectTransform.anchorMin = one_zero;
                        rectTransform.anchorMax = Vector2.one;
                    }
                    break;

                case EAnchorPreset.HorizontalStretchTop:
                    {
                        Vector2 zero_one = Vector2.zero;
                        zero_one.y = 1;
                        rectTransform.anchorMin = zero_one;
                        rectTransform.anchorMax = Vector2.one;
                    }
                    break;

                case EAnchorPreset.HorizontalStretchMiddle:
                    {
                        Vector2 zero_half = Vector2.zero;
                        zero_half.y = 0.5f;
                        Vector2 one_half = Vector2.one;
                        one_half.y = 0.5f;
                        rectTransform.anchorMin = zero_half;
                        rectTransform.anchorMax = one_half;
                    }
                    break;

                case EAnchorPreset.HorizontalStretchBottom:
                    {
                        Vector2 one_zero = Vector2.one;
                        one_zero.y = 0;
                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchorMax = one_zero;
                    }
                    break;

                case EAnchorPreset.DualStretch:
                    {
                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchorMax = Vector2.one;
                    }
                    break;
            }
        }

        /// <summary>
        /// Set anchor pivot.
        /// </summary>
        private static void _SetAnchorPivot(ref RectTransform rectTransform, EAnchorPivot anchorPivot)
        {
            switch (anchorPivot)
            {
                case EAnchorPivot.TopLeft:
                    {
                        Vector2 zero_one = Vector2.zero;
                        zero_one.y = 1;
                        rectTransform.pivot = zero_one;
                    }
                    break;

                case EAnchorPivot.TopCenter:
                    {
                        Vector2 half_one = Vector2.one;
                        half_one.x = 0.5f;
                        rectTransform.pivot = half_one;
                    }
                    break;

                case EAnchorPivot.TopRight:
                    {
                        rectTransform.pivot = Vector2.one;
                    }
                    break;

                case EAnchorPivot.MiddleLeft:
                    {
                        Vector2 zero_half = Vector2.zero;
                        zero_half.y = 0.5f;
                        rectTransform.pivot = zero_half;
                    }
                    break;

                case EAnchorPivot.MiddleCenter:
                    {
                        Vector2 half_half = Vector2.one * 0.5f;
                        rectTransform.pivot = half_half;
                    }
                    break;

                case EAnchorPivot.MiddleRight:
                    {
                        Vector2 one_half = Vector2.one;
                        one_half.y = 0.5f;
                        rectTransform.pivot = one_half;
                    }
                    break;

                case EAnchorPivot.BottomLeft:
                    {
                        rectTransform.pivot = Vector2.zero;
                    }
                    break;

                case EAnchorPivot.BottomCenter:
                    {
                        Vector2 half_zero = Vector2.zero;
                        half_zero.x = 0.5f;
                        rectTransform.pivot = half_zero;
                    }
                    break;

                case EAnchorPivot.BottomRight:
                    {
                        Vector2 one_zero = Vector2.one;
                        one_zero.y = 0;
                        rectTransform.pivot = one_zero;
                    }
                    break;
            }
        }

        #endregion

        #region ----- Enumeration -----

        public enum EAnchorPreset
        {
            Unknown,

            TopLeft,
            TopCenter,
            TopRight,

            MiddleLeft,
            MiddleCenter,
            MiddleRight,

            BottomLeft,
            BottomCenter,
            BottomRight,

            VerticalStretchLeft,
            VerticalStretchCenter,
            VerticalStretchRight,

            HorizontalStretchTop,
            HorizontalStretchMiddle,
            HorizontalStretchBottom,

            DualStretch
        }

        public enum EAnchorPivot
        {
            Unknown,

            TopLeft,
            TopCenter,
            TopRight,

            MiddleLeft,
            MiddleCenter,
            MiddleRight,

            BottomLeft,
            BottomCenter,
            BottomRight
        }

        #endregion
    }
}