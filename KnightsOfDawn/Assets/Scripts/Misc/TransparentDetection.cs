using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
{
    [Range(0,1)]
    [SerializeField] private float transparencyAmt = 0.8f;
    [SerializeField] private float fadeTime = 0.4f;

    private SpriteRenderer sr;
    private Tilemap tilemap;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D other) {  // adds the transparency to the object
        if (other.gameObject.GetComponent<PlayerController>()) {
            if (sr) {
                StartCoroutine(FadeRoutine(sr, fadeTime, sr.color.a, transparencyAmt));
            }
            else if (tilemap) {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmt));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) { // removes the transparency
        if (other.gameObject.GetComponent<PlayerController>()) {
            if (sr) {
                StartCoroutine(FadeRoutine(sr, fadeTime, sr.color.a, 1f));
            }
            else if (tilemap) {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));
            }
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer sr, float fadeT, float start, float targetTrans) {   // for sprites
        float elapsed = 0;
        while (elapsed < fadeT) {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(start, targetTrans, elapsed / fadeT);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);
            yield return null;
        }
    }

    private IEnumerator FadeRoutine(Tilemap tm, float fadeT, float start, float targetTrans) {  // for tiles
        float elapsed = 0;
        while (elapsed < fadeT) {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(start, targetTrans, elapsed / fadeT);
            tm.color = new Color(tm.color.r, tm.color.g,    tm.color.b, newAlpha);
            yield return null;
        }
    }
}
