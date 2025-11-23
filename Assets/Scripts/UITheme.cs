using UnityEngine;

public static class UITheme {
  public static readonly Color Blue    = new Color(0.08f,0.45f,0.78f,1f);   // Ready
  public static readonly Color Green   = new Color(0.05f,0.65f,0.45f,1f);   // Active
  public static readonly Color Yellow  = new Color(0.98f,0.78f,0.15f,1f);   // Paused
  public static readonly Color Red     = new Color(0.85f,0.20f,0.25f,1f);   // Error
  public static readonly Color Purple  = new Color(0.54f,0.30f,0.80f,1f);   // Save
  public enum SessionState { Ready, Active, Paused, Error, Complete }
  public static (string, Color) LabelFor(SessionState s) => s switch {
    SessionState.Ready    => ("STATUS: READY",   Blue),
    SessionState.Active   => ("STATUS: ACTIVE",  Green),
    SessionState.Paused   => ("STATUS: PAUSED",  Yellow),
    SessionState.Error    => ("STATUS: ERROR",   Red),
    SessionState.Complete => ("STATUS: COMPLETE",Green),
    _ => ("STATUS", Blue)
  };
}
