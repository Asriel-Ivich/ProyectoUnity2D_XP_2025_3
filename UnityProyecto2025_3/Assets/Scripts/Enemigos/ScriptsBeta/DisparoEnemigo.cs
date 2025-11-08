// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DisparoEnemigo : MonoBehaviour
// {
//     public Transform controladorDisparo;

//     public float distanciaLinea;

//    public LayerMask capaJugador;

//     public bool jugadorEnRango;

//     public float tiempoEntreDisparos;

//     public float tiempoUltimoDisparo;

//     public GameObject balaEnemigo;

//     public float tiempoEsperaDisparo;

//     private void Update()
//     {
//         jugadorEnRango = Physics2D.Raycast(controladorDisparo.position, Vector2.down, distanciaLinea, capaJugador);
// 
//         if (jugadorEnRango)
//         {
//             if (Time.time > tiempoEntreDisparos + tiempoUltimoDisparo)
//             {
//                 tiempoUltimoDisparo = Time.time;
//                 Invoke(nameof(Disparar), tiempoEsperaDisparo);
//             }
//         }
//    }
//   private void Disparar()
//   {
//        Instantiate(balaEnemigo, controladorDisparo.position, controladorDisparo.rotation);
//    }
//
//    private void OnDrawGizmos()
//   {
//       Gizmos.color = Color.red;
//       Gizmos.DrawLine(controladorDisparo.position,controladorDisparo.position + Vector3.down * distanciaLinea);
//   }
//}
