using UnityEngine;
using TMPro;
public class StatusIndicatorController : MonoBehaviour {
  public TMP_Text headerText;   // StatusHeaderText
  public TMP_Text footerText;   // StatusText (bottom of panel)
  [Range(0,1)] public float pulseSpeed = 2f;

  UITheme.SessionState state = UITheme.SessionState.Ready;
  public void SetState(UITheme.SessionState s, string tail = ""){
    state = s;
    var (label, color) = UITheme.LabelFor(s);
    if(headerText){ headerText.text = label; headerText.color = Color.white; }
    var img = GetComponent<UnityEngine.UI.Image>();
    if(img) img.color = color;
    if(footerText){
      footerText.text = tail switch {
        "running" => "▶ Exercise in Progress…",
        "paused"  => "⏸ Paused – press Start to resume",
        "done"    => "✅ Session Complete!",
        "warn"    => "⚠ Low accuracy – follow the avatar closely",
        _         => "⭕ Ready to Start"
      };
    }
  }
  void Update(){
    if(state == UITheme.SessionState.Ready){
      var img = GetComponent<UnityEngine.UI.Image>();
      if(!img) return;
      float a = 0.85f + 0.15f*Mathf.Sin(Time.time*pulseSpeed*3.14f);
      img.color = new Color(img.color.r, img.color.g, img.color.b, a);
    }
  }
}
