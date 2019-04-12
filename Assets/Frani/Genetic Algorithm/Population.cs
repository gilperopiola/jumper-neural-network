using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Population {
    public List<Individual> individuals;
    public List<List<Enemy>> enemies;

    public int nGeneration = 0;

    public Population() {
        individuals = new List<Individual>();
        enemies = new List<List<Enemy>>();

        for (var i = 0; i < ConfigManager.config.geneticAlgorithm.nIndividuals; i++) {
            individuals.Add(new Individual(i));

            enemies.Add(new List<Enemy>());
            float distanceOffset = 0;
            for (int j = 0; j < ConfigManager.config.nEnemies; j++) {
                enemies[i].Add(new Enemy(i, distanceOffset));
                distanceOffset = enemies[i][j].Distance;
            }

            individuals[i].SetEnemies(enemies[i]);
        }
    }

    public void Advance() {
        foreach (var enemyLayer in enemies) {
            foreach (var enemy in enemyLayer) {
                if (!individuals[enemy.IndividualIndex].Finished) {
                    enemy.Advance();
                }
            }
        }

        for (var i = 0; i < individuals.Count; i++) {
            if (!individuals[i].Finished) {
                individuals[i].Advance();
            }
        }

        foreach (var individual in individuals) {
            if (!individual.Finished) {
                individual.CheckCollision();
            }
        }

        if (HasFinished()) {
            Epoch();
        }
    }

    public void Epoch() {
        PrintStats();
        DestroyGameObjects();
        nGeneration++;

        individuals = individuals.OrderByDescending(o => o.Fitness).ToList();

        List<Individual> newIndividuals = new List<Individual>();
        newIndividuals.AddRange(CloneElite(ConfigManager.config.geneticAlgorithm.nElite));
        newIndividuals.AddRange(ReproductionHelper.Reproduce(individuals, ConfigManager.config.geneticAlgorithm.nIndividuals - ConfigManager.config.geneticAlgorithm.nElite, GetFitness()));
        MutationHelper.Mutate(newIndividuals);

        enemies = new List<List<Enemy>>();
        for (int i = 0; i < newIndividuals.Count; i++) {
            newIndividuals[i].Index = i;
            newIndividuals[i].CreateGameObject();

            enemies.Add(new List<Enemy>());
            float distanceOffset = 0;
            for (int j = 0; j < ConfigManager.config.nEnemies; j++) {
                enemies[i].Add(new Enemy(i, distanceOffset));
                distanceOffset = enemies[i][j].Distance;
            }
            newIndividuals[i].SetEnemies(enemies[i]);
        }

        individuals = new List<Individual>(newIndividuals);
        CreateGameObjects();
    }

    public List<Individual> CloneElite(int nElite) {
        List<Individual> elite = new List<Individual>();
        for (var i = 0; i < nElite; i++) {
            elite.Add(individuals[i].Clone());
        }
        return elite;
    }

    public int GetFitness() {
        int sum = 0;
        for (int i = 0; i < individuals.Count; i++) {
            sum += individuals[i].Fitness;
        }
        return sum;
    }

    public bool HasFinished() {
        foreach (var individual in individuals) {
            if (!individual.Finished) {
                return false;
            }
        }
        return true;
    }

    public void CreateGameObjects() {
        for (int i = 0; i < individuals.Count; i++) {
            individuals[i].CreateGameObject();
        }
    }

    public void DestroyGameObjects() {
        foreach (var enemyLayer in enemies) {
            foreach (var enemy in enemyLayer) {
                enemy.DestroyGameObject();
            }
        }

        for (int i = 0; i < individuals.Count; i++) {
            individuals[i].DestroyGameObject();
        }
    }

    public void PrintStats() {
        Debug.Log("Finished generation " + nGeneration + " with a total fitness of " + GetFitness());
    }

    public override string ToString() {
        string s = "Population: ";
        for (int i = 0; i < individuals.Count; i++) {
            s += individuals[i].ToString();
        }
        return s;
    }
}