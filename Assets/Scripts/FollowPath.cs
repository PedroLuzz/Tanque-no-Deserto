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
    public Button heliportoButton; // Bot�o para mover at� o heliporto
    public Button ruinasButton; // Bot�o para mover at� as ru�nas
    public Button fabricaButton; // Bot�o para mover at� a f�brica

    void Start()
    {
        // Obt�m as refer�ncias para os waypoints e o grafo do WPManager
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];

        // Configura os listeners dos bot�es
        heliportoButton.onClick.AddListener(() => GoToHeli());
        ruinasButton.onClick.AddListener(() => GoToRuin());
        fabricaButton.onClick.AddListener(() => GoToFabrica());
    }

    public void GoToHeli()
    {
        // Encontra o caminho at� o heliporto
        g.AStar(currentNode, wps[1]);
        currentWP = 0;
    }

    public void GoToRuin()
    {
        // Encontra o caminho at� as ru�nas
        g.AStar(currentNode, wps[6]);
        currentWP = 0;
    }

    public void GoToFabrica()
    {
        // Encontra o caminho at� a f�brica
        g.AStar(currentNode, wps[8]);
        currentWP = 0;
    }

    void LateUpdate()
    {
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
            return;

        // Define o n� atual mais pr�ximo
        currentNode = g.getPathPoint(currentWP);

        // Se estivermos pr�ximos o suficiente do n�, passamos para o pr�ximo
        if (Vector3.Distance(g.getPathPoint(currentWP).transform.position, transform.position) < accuracy)
        {
            currentWP++;
        }

        if (currentWP < g.getPathLength())
        {
            goal = g.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            // Rotaciona suavemente em dire��o ao pr�ximo waypoint
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);

            // Move-se em frente na dire��o do waypoint
            this.transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
