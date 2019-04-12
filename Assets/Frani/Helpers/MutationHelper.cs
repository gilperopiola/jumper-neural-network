using System.Collections.Generic;
using UnityEngine;

public static class MutationHelper {
    public static void Mutate(List<Individual> individuals) {
        for (var i = 0; i < individuals.Count; i++) {
            Mutate(individuals[i]);
        }
    }

    public static void Mutate(Individual individual) {
        for (var i = 0; i < individual.Dna.weights.Count; i++) {
            if (RandomGenerator.Float(0, 100) < ConfigManager.config.geneticAlgorithm.mutationPercentage) {
                Mutate(individual.Dna.weights[i]);
            }
        }
    }

    public static void Mutate(float weight) {
        weight *= RandomGenerator.Float(0.8f, 1.2f);
    }
}