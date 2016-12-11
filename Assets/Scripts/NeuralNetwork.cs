using System.Collections.Generic;

public class NeuralNetwork
{
    private Matrix _inputLayer;
    private Matrix _hiddenLayer;
    private Matrix _outputLayer;

    public NeuralNetwork(int size)
    {

        _inputLayer = Matrix.RandomMat(1, size + 1);
        _hiddenLayer = Matrix.RandomMat(_inputLayer.y, 25);
        _outputLayer = Matrix.RandomMat(_hiddenLayer.y, 2);
    }

    public NeuralNetwork(NeuralNetwork n)
    {
        List<Matrix> l = n.Layers();
        _inputLayer = l[0];
        _hiddenLayer = l[1];
        _outputLayer = l[2];
    }

    public int Predict(List<float> input)
    {
        input.Add(1.0f);
        Matrix inputMatrix = new Matrix(input.Count, 1);
        for (var i = 0; i < input.Count; i++)
        {
            inputMatrix[i, 0] = input[i];
        }

        Matrix l1 = inputMatrix * _inputLayer;
        l1.sigmoid();
        Matrix l2 = l1 * _hiddenLayer;
        l2.tanh();
        Matrix output = l2 * _outputLayer;
        l2.sigmoid();

        int index = 2;
        float val = 0.5f;
        for (int i = 0; i < output.y; i++)
        {
            if (output[0, i] > val)
            {
                index = i;
            }
        }
        return index;

    }

    List<Matrix> Layers()
    {
        return new List<Matrix>()
        {
            _inputLayer.Copy,
            _hiddenLayer.Copy,
            _outputLayer.Copy
        };
    }

    public void Mutate()
    {
        float evolve_rate = 0.2f;
        _inputLayer.Mutate(evolve_rate);
        _hiddenLayer.Mutate(evolve_rate);
        _outputLayer.Mutate(evolve_rate);
    }
}
