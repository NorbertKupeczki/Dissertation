using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class AgentGenerator : MonoBehaviour
{

    [Header("Generatl Settings")]
    [SerializeField] private int _minNumberOfAgents = 3;
    [SerializeField] private int _maxNumberOfAgents = 10;
    [SerializeField] private int _numberOfClusters = 3;

    [SerializeField] private GameObject _agentTemplate;
    
    private Dictionary<int, Color> _hairColours = new Dictionary<int, Color>
    {
        { 0, new Color(0.17f, 0.13f, 0.17f) },
        { 1, new Color(0.42f, 0.31f, 0.29f) },
        { 2, new Color(0.74f, 0.59f, 0.47f) },
        { 3, new Color(0.55f, 0.29f, 0.26f) },
        { 4, new Color(0.79f, 0.32f, 0.22f) },
        { 5, new Color(0.65f, 0.42f, 0.27f) },
        { 6, new Color(0.84f, 0.77f, 0.76f) }
    };
    private Dictionary<int, Color> _skinColours = new Dictionary<int, Color>
    {
        { 0, new Color(1.00f, 0.86f, 0.45f) },
        { 1, new Color(0.89f, 0.63f, 0.29f) },
        { 2, new Color(0.80f, 0.52f, 0.26f) },
        { 3, new Color(0.73f, 0.42f, 0.29f) },
        { 4, new Color(0.21f, 0.00f, 0.00f) },
        { 5, new Color(0.88f, 0.68f, 0.64f) }
    };

    private const int MAX_ROW = 3;
    private bool processDone = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !processDone)
        {
            processDone = true;
            GenerateAgents();
        }
    }

    private Color GetRandomHairColour()
    {
        return _hairColours[Random.Range(0, _hairColours.Count)];
    }

    private Color GetRandomSkinColour()
    {
        return _skinColours[Random.Range(0, _skinColours.Count)];
    }

    private void GenerateAgents()
    {
        Vector3 startPosition = Vector3.zero;
        
        for (int i = 0; i < _numberOfClusters; i++)
        {
            GenerateCluster(ref startPosition, i);

            if ((i + 1) % 3 == 0)
            {
                startPosition = Vector3.zero + (Vector3.forward * 10.0f) * (i + 1) / MAX_ROW;
            }
            else
            {
                startPosition += Vector3.right * 10.0f;
            }
        }
    }

    private GameObject GenerateCluster(ref Vector3 clusterPosition, int index)
    {
        int numberOfAgents = Random.Range(_minNumberOfAgents, _maxNumberOfAgents + 1);

        GameObject cluster = new GameObject();
        cluster.AddComponent<Cluster>();
        cluster.transform.position = clusterPosition;

        cluster.name = "Cluster " + (index + 1);

        FillClusterWithAgents(cluster.GetComponent<Cluster>(), numberOfAgents);

        return cluster;
    }

    private void FillClusterWithAgents(Cluster cluster, int numberOfAgents)
    {
        float clusterRadius = (2.0f * numberOfAgents) / (2 * Mathf.PI);
        Vector3 radiusVector = new Vector3(clusterRadius, 0.0f, 0.0f);
        float clusterSegmentInDegrees = 360 / numberOfAgents;

        for (int i = 0; i < numberOfAgents; i++)
        {
            Vector3 agentPosition = (Quaternion.Euler(0, clusterSegmentInDegrees * i, 0) * radiusVector) + cluster.transform.position;
            GameObject agent = Instantiate(_agentTemplate, agentPosition, Quaternion.identity, cluster.transform);
            agent.GetComponent<Agent>().InitAgent(GetRandomHairColour(), GetRandomSkinColour(), cluster._clusterColour);
            agent.transform.LookAt(cluster.transform.position, Vector3.up);
            agent.name = "Agent no." + (i + 1);

            cluster.RegisterMember(agent.GetComponent<Agent>());
        }

        cluster.GenerateConnections();
    }
}
