using System.Collections.Generic;
using UnityEngine;

public class Genetics
{
    private List<Agent> _agents;
    private int _gen;

    public Genetics()
    {
        _agents = new List<Agent>();
        _gen = 1;
    }

    public void AddAgent(Agent a)
    {
        _agents.Add(a);
    }

    public void Evolve()
    {
        Agent best = _agents[0];

        foreach (var agent in _agents)
        {
            if (agent.Score > best.Score)
            {
                best = agent;
            }
        }

        NeuralNetwork best_brains = best.Brains;
        float score = best.Score * 1.1f; //0.9f;

        foreach (var agent in _agents)
        {
            if (Random.Range(0.0f, 1.0f) < 0.3)
            {
                var brains = new NeuralNetwork(best_brains);
                brains.Mutate();
                agent.Brains = brains;
            }
        }
        _gen++;

    }

    public void Reset(Vector2 position)
    {
        foreach (var agent in _agents)
        {
            agent.Reset(position);
        }
    }

    public int Alive()
    {
        int alive = 0;
        foreach (var agent in _agents)
        {
            if (agent.Alive)
                alive++;
        }
        return alive;
    }

    public float Score
    {
        get
        {
            float score = 0;
            foreach (var agent in _agents)
            {
                if (agent.Score > score)
                {
                    score = agent.Score;
                }
            }
            return score;
        }
    }

    public int Generation
    {
        get
        {
            return _gen;
            
        }
    }
}
