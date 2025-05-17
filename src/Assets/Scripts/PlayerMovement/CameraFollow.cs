using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   // Le transform du joueur à suivre
    public float smoothSpeed = 0.125f;  // Vitesse de "lissage"
    public Vector3 offset;  // Décalage par rapport au joueur (ex : un peu au-dessus)

    // Si le joueur n'est pas encore attribué au début, on va essayer de le récupérer.
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;  // Recherche du joueur par tag
        }
    }

    void LateUpdate()
    {
        // Assurez-vous que player existe
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset; // Calculer la position souhaitée avec offset
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Appliquer un lissage
            smoothedPosition.z = -10;
            transform.position = smoothedPosition;  // Déplacer la caméra à la nouvelle position
        }
        else
            player = GameObject.FindWithTag("Player")?.transform;  // Recherche du joueur par tag
    }
}
