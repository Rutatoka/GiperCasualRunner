//using UnityEngine;

//public class Billboard : MonoBehaviour
//{
//    public Transform player;
//    public bool isTree = false;

//    void Start()
//    {
//        // Ищем игрока если не назначен
//        if (player == null)
//        {
//            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
//            if (playerObj != null) player = playerObj.transform;
//        }
//    }

//    void LateUpdate()
//    {
//        if (player == null) return;

//        // Поворачиваем объект так, чтобы его "лицо" (forward) смотрело на игрока
//        Vector3 direction = player.position - transform.position;
//        direction.y = 0; // Не наклоняем вверх-вниз

//        if (direction != Vector3.zero)
//        {
//            // Создаем поворот, глядящий на игрока
//            Quaternion targetRotation = Quaternion.LookRotation(direction);

//            // Для 2D спрайтов нужно, чтобы они смотрели на камеру своим "лицом"
//            // Обычно спрайты смотрят в сторону +Z или +X
//            // Пробуем разные варианты:

//            // Вариант A: Если спрайт смотрит в +Z
//            // transform.rotation = targetRotation;

//            // Вариант B: Если спрайт смотрит в +X (как картонка)
//            transform.rotation = targetRotation * Quaternion.Euler(0, 90, 0);

//            // Вариант C: Если спрайт двусторонний (виден с обеих сторон)
//            // transform.rotation = targetRotation * Quaternion.Euler(0, 0, 0);
//        }
//    }
//}