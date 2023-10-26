using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AGL.Player
{
    public class PlayerSpaceSuitUIView : MonoBehaviour
    {
        [SerializeField]
        private PlayerSpaceSuit suit;

        [SerializeField]
        private Image oxygenBar;

        [SerializeField]
        private TMP_Text suitIntegrityText;

        [SerializeField]
        private Gradient oxygenGradient;

        [SerializeField]
        private Gradient integrityGradient;

        private void Update()
        {
            oxygenBar.fillAmount = Mathf.Lerp(oxygenBar.fillAmount, suit.OxygenPercent, 7 * Time.deltaTime);
            oxygenBar.color = oxygenGradient.Evaluate(1 - suit.OxygenPercent);

            suitIntegrityText.SetText($"{(int) suit.Integrity}");
            suitIntegrityText.color = integrityGradient.Evaluate(1 - suit.IntegrityPercent);
        }
    }
}
