using UnityEngine;

public class Enemy {
    public GameObject GameObject { get; set; }
    public int IndividualIndex { get; set; }
    public float Distance { get; set; }

    public float Speed;

    public Enemy(int individualIndex, float distanceOffset) {
        IndividualIndex = individualIndex;
        Distance = distanceOffset + RandomGenerator.Float(200, 800);
        Speed = RandomGenerator.Float(1.3f, 8);
        CreateGameObject();
    }

    public void Advance() {
        GameObject.transform.position -= new Vector3(Speed, 0, 0);
    }

    public void CreateGameObject() {
        GameObject = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/Enemy"), new Vector3(Distance, 250 - IndividualIndex * 64, 0), Quaternion.identity);
    }

    public void DestroyGameObject() {
        GameObject.Destroy(GameObject);
    }
}

