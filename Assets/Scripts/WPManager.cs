using UnityEngine;

[System.Serializable]
public struct Link
{
    public enum direction { UNI, BI }
    public GameObject node1;
    public GameObject node2;
    public direction dir;
}

public class WPManager : MonoBehaviour
{
    public GameObject[] waypoints;  // Array que armazena os waypoints
    public Link[] links;  // Array de links entre os waypoints
    public Graph graph = new Graph();  // Instância do grafo

    void Start()
    {
        if (waypoints.Length > 0)
        {
            // Adiciona cada waypoint como um nó no grafo
            foreach (GameObject wp in waypoints)
            {
                graph.AddNode(wp);
            }

            // Adiciona as arestas (links) entre os waypoints no grafo
            foreach (Link l in links)
            {
                graph.AddEdge(l.node1, l.node2);

                // Se a direção do link for bidirecional, adiciona uma aresta de volta
                if (l.dir == Link.direction.BI)
                    graph.AddEdge(l.node2, l.node1);
            }
        }
    }

    void Update()
    {
        // Desenha o grafo (apenas para fins de debugging)
        graph.debugDraw();
    }
}
