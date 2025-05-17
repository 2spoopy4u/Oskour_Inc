using UnityEngine;

public class CubeCollider : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(((int) EnumTerrain.Block).ToString()))
        {
            Player player = transform.parent.GetComponent<Player>();

            player.Die();
        }
    }
}