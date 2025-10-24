//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DisparoEnemigo : MonoBehaviour
//{
//    public Transform controladorDisparo;

//    public float distanciaLinea;

//    public LayerMask capaJugador;

//    public bool jugadorEnRango;

//    private void Update()
//    {
//        jugadorEnRango = Physics2D.Raycast(controladorDisparo.position, transform.rigth, distanciaLinea, capaJugador);

//        if (jugadorEnRango) { }
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawLine(controladorDisparo.position, controladorDisparo.position + transform.rigth * distanciaLinea);
//    }
//}
