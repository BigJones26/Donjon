using UnityEngine;

public class PlayerController : MonoBehaviour
{
    DungeonLevel1 collision;
    public bool smoothTransition = false;
    public float transitionSpeed = 10f;
    public float transitionRotationSpeed = 500f;

    Vector3 targetGridPos;
    Vector3 prevTargetGridPos;
    Vector3 targetRotation;

    private void Start() {

        collision = GetComponent<DungeonLevel1>();
        targetGridPos = Vector3Int.RoundToInt(transform.position);

    }

    private void FixedUpdate() {

        MovePlayer();

    }

    void MovePlayer() {

        if (true) {

            prevTargetGridPos = targetGridPos;

            Vector3 targetPosition = targetGridPos;

            if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
            if (targetRotation.y < 0f) targetRotation.y = 270f;

            if (!smoothTransition) {
                transform.position = targetPosition;
                transform.rotation = Quaternion.Euler(targetRotation);
            } else {

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
            }

        } else {


            targetGridPos = prevTargetGridPos;

        }


    }

    public void RotateLeft() { if (AtRest) targetRotation -= Vector3.up * 90f; }
    public void RotateRight() { if (AtRest) targetRotation += Vector3.up * 90f; }
    public void MoveForward() {
        if (AtRest) {
            Vector3 newPos = targetGridPos + transform.forward;
            
            if (collision.IsWalkable(newPos)) {
                targetGridPos = newPos;
            }
        }
    }
    public void MoveBackward() {
        if (AtRest) {
            Vector3 newPos = targetGridPos - transform.forward;
            
            if (collision.IsWalkable(newPos)) {
                targetGridPos = newPos;
            }
        }
    }
    public void MoveLeft() {
        if (AtRest) {
            Vector3 newPos = targetGridPos - transform.right;
            
            if (collision.IsWalkable(newPos)) {
                targetGridPos = newPos;
            }
        }
    }
    public void MoveRight() {
        if (AtRest) {
            Vector3 newPos = targetGridPos + transform.right;
            
            if (collision.IsWalkable(newPos)) {
                targetGridPos = newPos;
            }
        }
    }

    bool AtRest {
        get {

            if ((Vector3.Distance(transform.position, targetGridPos) < 0.05f) &&
                (Vector3.Distance(transform.eulerAngles, targetRotation) < 0.05f))
                return true;
            else 
                return false;
        }
    }
}