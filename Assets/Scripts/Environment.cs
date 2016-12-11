using UnityEngine;
using UnityEngine.UI;

public class Environment : MonoBehaviour
{

    public Transform StartPosition;
    public Text Dashboard;

    private Genetics _genetics;
    private float _bestAllTimeScore;
    private float _bestCurrentScore;

	// Use this for initialization
	void Start () {
        _genetics = new Genetics();
	    var agents = FindObjectsOfType<Agent>();
	    foreach (var agent in agents)
	    {
	        _genetics.AddAgent(agent);
	    }
	    _bestAllTimeScore = 0;
	    _bestCurrentScore = 0;



	}
	
	// Update is called once per frame
	void Update ()
	{
	    int alive = _genetics.Alive();
        if (alive == 0)
	    {
	        Evolve();
	    }
	    float score = _genetics.Score * 10;
	    if (score > _bestCurrentScore)
	    {
	        _bestCurrentScore = _genetics.Score;
	    }
	    if (_bestCurrentScore > _bestAllTimeScore)
	    {
	        _bestAllTimeScore = _bestCurrentScore;
	    }

	    string text = "";
	    text += "Generation: " + _genetics.Generation + "\n";
	    text += "Score: " + (int)_bestCurrentScore + "\n";
        text += "Best score: " + (int)_bestAllTimeScore + "\n";
        text += "Alive: " + alive + "\n";

        Dashboard.text = text;
	}

    public void Evolve()
    {
        Vector2 pos = StartPosition.position;
        _genetics.Evolve();
        _genetics.Reset(pos);
        _bestCurrentScore = 0;
    }
}
