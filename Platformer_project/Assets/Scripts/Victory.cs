using UnityEngine;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    [SerializeField] Text text;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            text.gameObject.SetActive(true);
        }
    }
}
