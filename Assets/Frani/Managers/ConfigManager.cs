public static class ConfigManager {
    public static Config config;

    public static void Init(string fileName) {
        config = UnityEngine.JsonUtility.FromJson<Config>(FileHandler.Read(fileName));
    }

    [System.Serializable]
    public class Config {
        public string projectName;
        public bool debugMode;
        public int nEnemies;
        public GeneticAlgorithmConfig geneticAlgorithm;
        public NeuralNetworkConfig neuralNet;
    }

    [System.Serializable]
    public class TileMapConfig {
        public string fileName;
        public int width;
        public int height;
    }

    [System.Serializable]
    public class GeneticAlgorithmConfig {
        public int nIndividuals;
        public int nElite;
        public float mutationPercentage;
        public int nMovements;
    }

    [System.Serializable]
    public class NeuralNetworkConfig {
        public int nInputNeurons;
        public int nHiddenNeurons;
        public int nOutputNeurons;
        public int nHiddenLayers;
    }
}
