using UnityEngine;

namespace AppLayer {
    public abstract class AObject : MonoBehaviour {

        protected AudioSource _audio;
        
        public Vector3 Coordinates { 
            get => transform.position;
            protected set {
                if(transform.position.x > 9.5f) {
                    transform.position = new Vector3(-9.5f, transform.position.y, 0);
                }
                else if(transform.position.x < -9.5f) {
                    transform.position = new Vector3(9.5f, transform.position.y, 0);
                }
                else if(transform.position.y > 5) {
                    transform.position = new Vector3(transform.position.x, -5, 0);
                }
                else if(transform.position.y < -5) {
                    transform.position = new Vector3(transform.position.x, 5, 0);
                }
                else {
                    transform.position = value;
                }
            }
        }
        public Vector3 Direction { get; protected set; }
        
        public abstract void OnTriggerEnter2D(Collider2D enteredCollider);
        public abstract void Destroy();
    }
}
