using System.Collections.Generic;

public static class PopulationHistory {
    public static List<List<Individual>> individualsHistory;
    public static List<int> fitnessHistory;

    public static void AddIndividuals(List<Individual> individuals) {
        if (individualsHistory == null) {
            individualsHistory = new List<List<Individual>>();
        }
        if (fitnessHistory == null) {
            fitnessHistory = new List<int>();
        }

        individualsHistory.Add(individuals);

        int totalFitness = 0;
        for (var i = 0; i < individuals.Count; i++) {
            totalFitness += individuals[i].Fitness;
        }
        fitnessHistory.Add(totalFitness);
    }

    public static List<Individual> GetBest() {
        int maxFitness = int.MinValue;
        int index = 0;
        for (int i = 0; i < fitnessHistory.Count; i++) {
            if (fitnessHistory[i] > maxFitness) {
                maxFitness = fitnessHistory[i];
                index = i;
            }
        }
        return individualsHistory[index];
    }
}