using UnityEngine;
using UnityEngine.UI;

public class FollowPath : MonoBehaviour
{
    Transform goal;
    public float speed = 5.0f;
    float accuracy = 1.0f;
    float rotSpeed = 2.0f;
    public GameObject wpManager;
    GameObject[] wps;
    GameObject currentNode;
    int currentWP = 0;
    Graph g;
    public Button heliportoButton; // Botão para mover até o heliporto
    public Button ruinasButton; // Botão para mover até as ruínas
    public Button fabricaButton; // Botão para mover até a fábrica

    void Start()
    {
        // Obtém as referências para os waypoints e o grafo do WPManager
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];

        // Configura os listeners dos botões
        heliportoButton.onClick.AddListener(() => GoToHeli());
        ruinasButton.onClick.AddListener(() => GoToRuin());
        fabricaButton.onClick.AddListener(() => GoToFabrica());
    }

    public void GoToHeli()
    {
        // Encontra o caminho até o heliporto
        g.AStar(currentNode, wps[1]);
        currentWP = 0;
    }

    public void GoToRuin()
    {
        // Encontra o caminho até as ruínas
        g.AStar(currentNode, wps[6]);
        currentWP = 0;
    }

    public void GoToFabrica()
    {
        // Encontra o caminho até a fábrica
        g.AStar(currentNode, wps[8]);
        currentWP = 0;
    }

    void LateUpdate()
    {
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
            return;

        // Define o nó atual mais próximo
        currentNode = g.getPathPoint(currentWP);

        // Se estivermos próximos o suficiente do nó, passamos para o próximo
        if (Vector3.Distance(g.getPathPoint(currentWP).transform.position, transform.position) < accuracy)
        {
            currentWP++;
        }

        if (currentWP < g.getPathLength())
        {
            goal = g.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            // Rotaciona suavemente em direção ao próximo waypoint
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);

            // Move-se em frente na direção do waypoint
            this.transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
