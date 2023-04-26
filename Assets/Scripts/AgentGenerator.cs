using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Utility.Utility;

public class AgentGenerator : MonoBehaviour
{

    [Header("Generatl Settings")]
    [SerializeField] private int _minNumberOfAgents = 3;
    [SerializeField] private int _maxNumberOfAgents = 10;
    [SerializeField] private int _numberOfClusters = 3;
    [Space]
    [Header("Prefab")]
    [SerializeField] private GameObject _agentTemplate;
    [Space]
    [Header("Clusters")]
    [SerializeField] private List<Cluster> _clusters = new();
    
    private const int MAX_ROW = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GenerateButtonInput()
    {
        GenerateAgents();
    }

    private void GenerateAgents()
    {
        Vector3 startPosition = Vector3.zero;
        
        for (int i = 0; i < _numberOfClusters; i++)
        {
            _clusters.Add(GenerateCluster(ref startPosition, i));

            if ((i + 1) % 3 == 0)
            {
                startPosition = Vector3.zero + (Vector3.forward * 10.0f) * (i + 1) / MAX_ROW;
            }
            else
            {
                startPosition += Vector3.right * 10.0f;
            }
        }

        GenerateInterClusterConnections();

        InitialiseGossipModules();
    }

    private void InitialiseGossipModules()
    {
        foreach (Cluster cluster in _clusters)
        {
            foreach (Agent agent in cluster.GetMembersList())
            {
                agent.InitGossipModule();
            }
        }
    }

    private void GenerateInterClusterConnections()
    {
        foreach (Cluster cluster in _clusters)
        {
            cluster.GenerateInterClusterConnections(_clusters);
        }
    }

    private Cluster GenerateCluster(ref Vector3 clusterPosition, int index)
    {
        int numberOfAgents = Random.Range(_minNumberOfAgents, _maxNumberOfAgents + 1);

        GameObject cluster = new GameObject();
        Cluster scriptComponent = cluster.AddComponent<Cluster>();
        cluster.transform.position = clusterPosition;

        cluster.name = "Cluster " + (index + 1);

        FillClusterWithAgents(scriptComponent, numberOfAgents, index);

        return scriptComponent;
    }

    private void FillClusterWithAgents(Cluster cluster, int numberOfAgents, int clusterID)
    {
        float clusterRadius = (2.0f * numberOfAgents) / (2 * Mathf.PI);
        Vector3 radiusVector = new Vector3(clusterRadius, 0.0f, 0.0f);
        float clusterSegmentInDegrees = 360 / numberOfAgents;

        for (int i = 0; i < numberOfAgents; i++)
        {
            Vector3 agentPosition = (Quaternion.Euler(0, clusterSegmentInDegrees * i, 0) * radiusVector) + cluster.transform.position;
            GameObject agent = Instantiate(_agentTemplate, agentPosition, Quaternion.identity, cluster.transform);
            agent.GetComponent<Agent>().InitAgent(GetRandomHairColour(), GetRandomSkinColour(), cluster._clusterColour, clusterID);
            agent.transform.LookAt(cluster.transform.position, Vector3.up);
            agent.name = "Agent no." + (i + 1);

            cluster.RegisterMember(agent.GetComponent<Agent>());
        }

        cluster.GenerateConnections();
    }
}
