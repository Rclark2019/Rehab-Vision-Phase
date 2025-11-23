using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadialTimer : MonoBehaviour {
  public Image ring; public TMP_Text label;
  float duration, t; bool active;
  public void Begin(float seconds){ duration = Mathf.Max(0.01f,seconds); t=0; active=true; gameObject.SetActive(true); }
  public void Stop(){ active=false; gameObject.SetActive(false); }
  void Update(){
    if(!active) return;
    t += Time.deltaTime;
    float r = Mathf.Clamp01(1f - t/duration);
    if(ring) ring.fillAmount = r;
    if(label) label.text = Mathf.CeilToInt(duration - t).ToString() + "s";
    if(t>=duration) Stop();
  }
}
