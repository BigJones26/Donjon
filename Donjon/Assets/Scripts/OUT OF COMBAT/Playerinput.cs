using UnityEngine;
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(DungeonLevel1))]

public class NewMonoBehaviour : MonoBehaviour
{
    public KeyCode forward = KeyCode.W;
    public KeyCode back = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode turnLeft = KeyCode.Q;
    public KeyCode turnRight = KeyCode.E;

    PlayerController controller;
    DungeonLevel1 collision;
    private void Awake() {
        controller = GetComponent<PlayerController>();
        collision = GetComponent<DungeonLevel1>();
    }
    private void Update() {

        if (Input.GetKeyUp(forward)) {
            controller.MoveForward();
        }
        if (Input.GetKeyUp(back)) {
            controller.MoveBackward();
        }
        if (Input.GetKeyUp(left)) {
            controller.MoveLeft();
        }
        if (Input.GetKeyUp(right)) {
            controller.MoveRight();
        }
        if (Input.GetKeyUp(turnLeft)) {
            controller.RotateLeft();
        }
        if (Input.GetKeyUp(turnRight)) {
            controller.RotateRight();
        }
    }
    
}
