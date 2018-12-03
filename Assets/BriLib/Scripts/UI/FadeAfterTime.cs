using UnityEngine;

namespace BriLib
{
    public class FadeAfterTime : MonoBehaviour
    {
        public Fadeable UI;
        public float TimeBeforeFade;
        private float _liveTime;

        private void OnEnable()
        {
            _liveTime = 0f;
        }

        private void Update()
        {
            if (UI.Hiding) return;

            _liveTime += Time.deltaTime;
            if (_liveTime >= TimeBeforeFade) UI.Hide();
        }
    }
}
