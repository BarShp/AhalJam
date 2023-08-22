using System.Globalization;
using AHL.Core.General.Utils;
using AHL.Core.Main;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AHL.Core.Loaders
{
    public class AHLProgressLoaderBar : AHLMonoBehaviour
    {
        private const string MAX_TEXT = "MAX";

        [SerializeField] protected Image baseFill;
        [SerializeField] protected Image completeFill;
        [SerializeField] private TMP_Text counterText;
        [SerializeField] private TMP_Text totalText;
        [SerializeField] private GameObject completeBarContainer;

        [Header("Parameters")]
        [SerializeField] protected float completeFillTime = 2f;
        [SerializeField] protected Ease easeType;

        private float target;
        private float max;

        public void SetMaxBar(float maxFill, bool setFill = true)
        {
            if(maxFill < 0)
            {
                SwitchToCompleteView();
                target = max = 1;
                totalText.SetText(MAX_TEXT);
            }
            else
            {
                SwitchToIncompleteView();
                target = max = maxFill;
                totalText.SetText(max.ToString(CultureInfo.InvariantCulture));
            }

            if (setFill)
            {
                SetFillImmediate(target);
            }
        }

        public void FillTo(int from, int to)
        {
            SetFillImmediate(from);

            target = to < max ? to : max;
            var percentage = target / max;
            var countTime = completeFillTime;
            var fillTime = completeFillTime * ((target - from) / (to - from));

            fillTime = Mathf.Clamp(fillTime, completeFillTime / 4f, completeFillTime);

            if(target == max)
            {
                WaitForTimeSeconds(fillTime, SwitchToCompleteView);
            }

            var effectSequence = DOTween.Sequence();
            effectSequence.Join(counterText.DOCounter(from, to, countTime));
            effectSequence.Join(baseFill.DOFillAmount(percentage, fillTime));
        }

        private void SwitchToIncompleteView()
        {
            completeBarContainer.SetActive(false);
            baseFill.gameObject.SetActive(true);
        }

        private void SwitchToCompleteView()
        {
            baseFill.gameObject.SetActive(false);
            completeBarContainer.SetActive(true);
            completeFill.fillAmount = 1f;
        }

        public void SetFillImmediate(float to)
        {
            counterText.text = to.ToString("N0");

            if(to >= max)
            {
                SwitchToCompleteView();
                return;
            }

            SwitchToIncompleteView();
            float percentage = to / max;
            baseFill.fillAmount = percentage;
        }

#if UNITY_EDITOR
        public void TestMethod()
        {
            SetMaxBar(100, false);
            FillTo(50, 150);
        }
#endif
    }
}
