using AppLayer;
using UnityEngine;

namespace Services.Handling {
    public abstract class Handling {
        public HandlingTypes HandlingType;

        public abstract void AddForce(Rigidbody2D obj, float speed);
        public abstract void AddTorque(Rigidbody2D obj, float speed);
        public abstract bool Attack();
    }

    public enum HandlingTypes {
        KEYBOARD, MOUSE_KEYBOARD
    }
}
