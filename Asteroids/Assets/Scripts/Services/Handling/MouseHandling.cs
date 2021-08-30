using UnityEngine;

namespace Services.Handling {
    public class MouseHandling : Handling {
        
        public override void AddForce(Rigidbody2D obj, float speed) {
            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.W)) {
                obj.AddForce(obj.gameObject.transform.up * speed);
            }
        }
        public override void AddTorque(Rigidbody2D obj, float speed) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);     
            float angle = Mathf.Atan2(mousePosition.x - obj.gameObject.transform.position.x, 
                mousePosition.y - obj.gameObject.transform.position.y) * 180 / Mathf.PI;
            float angle1;
            
            obj.rotation %= 360;
            angle = angle + obj.rotation;
            if (angle < 0) {
                angle1 = 360.0f + angle;
            }
            else {
                angle1 = 360.0f - angle;
            }

            if (Mathf.Abs(angle) > Mathf.Abs(angle1) && angle < 0) {
                angle = angle1;
                
            }
            if (Mathf.Abs(angle) > Mathf.Abs(angle1) && angle > 0) {
                angle = angle1 * -1;
                
            }
            
            obj.AddTorque(-angle / 180 * speed);
        }
        public override bool Attack() {
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
        }
        
    }
}
