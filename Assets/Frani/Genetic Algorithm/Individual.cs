using UnityEngine;
using System.Collections.Generic;

public class Individual {
    public int Index { get; set; }
    public int Fitness = 0;
    public bool Finished = false;

    public bool Jumping = false;
    public float JumpTimer { get; set; }

    public bool Staggered = false;
    public float StaggeredTimer { get; set; }

    public NeuralNetwork NeuralNet { get; set; }
    public GameObject GameObject { get; set; }
    public DNA Dna { get; set; }
    public List<Enemy> Enemies { get; set; }

    public Individual(int index) {
        NeuralNet = new NeuralNetwork(new List<float>());
        Dna = new DNA(NeuralNet.GetWeights());
        Index = index;

        CreateGameObject();
    }

    public Individual(DNA dna) {
        Dna = dna;
        NeuralNet = new NeuralNetwork(Dna.weights);
    }

    public void Advance() {
        Fitness++;

        if (Jumping) {
            JumpTimer++;

            if (JumpTimer >= 40) {
                Jumping = false;
                JumpTimer = 0;
                GameObject.transform.position -= new Vector3(3, 24, 0);
                Staggered = true;
            }

            return;
        }

        if (Staggered) {
            StaggeredTimer++;

            if (StaggeredTimer >= 20) {
                Staggered = false;
                StaggeredTimer = 0;
            }

            return;
        }

        List<float> inputs = GetInputs();
        List<float> outputs = NeuralNet.Compute(inputs);

        if (outputs[0] >= 0.5f) {
            Jump();
        }
    }

    public void CheckCollision() {
        Enemy closestEnemy = GetClosestEnemy();

        if (CollisionManager.Squares(GameObject, closestEnemy.GameObject, 16)) { //checks for collision
            Finish();
        }
    }

    public List<float> GetInputs() {
        List<float> inputs = new List<float>();

        Enemy closestEnemy = GetClosestEnemy();

        inputs.Add(Mathf.Abs(GameObject.transform.position.x - closestEnemy.GameObject.transform.position.x) / 100);
        inputs.Add(closestEnemy.Speed);

        return inputs;
    }

    public Enemy GetClosestEnemy() {
        float minDistance = float.MaxValue;
        Enemy closestEnemy = null;

        foreach (Enemy enemy in Enemies) {
            float distance = Mathf.Abs(GameObject.transform.position.x - enemy.GameObject.transform.position.x);
            if (distance < minDistance) {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    public void Finish() {
        Finished = true;
    }

    public void Jump() {
        Jumping = true;
        GameObject.transform.position += new Vector3(3, 24, 0);
    }

    public Individual Clone() {
        Individual clone = new Individual(Dna);
        return clone;
    }

    public void SetEnemies(List<Enemy> enemies) {
        Enemies = enemies;
    }

    public void CreateGameObject() {
        GameObject = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/Individual"), new Vector3(-600, 250 - Index * 64, 0), Quaternion.identity);
    }

    public void DestroyGameObject() {
        GameObject.Destroy(GameObject);
    }
}