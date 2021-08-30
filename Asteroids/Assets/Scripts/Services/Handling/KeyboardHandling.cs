
using UnityEngine;

namespace Services.Handling {
    public class KeyboardHandling : Handling {
        
        public override void AddForce(Rigidbody2D obj, float speed) {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                obj.AddForce(obj.gameObject.transform.up * speed);
            }
            obj.velocity = Vector2.ClampMagnitude(obj.velocity, speed * 5);
        }
        public override void AddTorque(Rigidbody2D obj, float speed) {
            float turnDirection;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                turnDirection = 1f;
                obj.angularVelocity = Mathf.Clamp(obj.angularVelocity, 0, speed * 500);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                turnDirection = -1f;
                obj.angularVelocity = Mathf.Clamp(-obj.angularVelocity, 0, speed * -500);
            }
            else {
                turnDirection = 0f;
            }
            obj.AddTorque(turnDirection * speed);
        }
        public override bool Attack() {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }
}
