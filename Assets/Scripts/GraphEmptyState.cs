using UnityEngine;
using TMPro;

public class GraphEmptyState : MonoBehaviour {
  public TMP_Text emptyText; 
  public PerformanceGraphTexture graph;
  void LateUpdate(){
    if(!graph || !emptyText) return;
    emptyText.gameObject.SetActive(!graph.HasSamples);
  }
}
