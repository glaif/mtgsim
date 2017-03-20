using UnityEngine;
using System.Collections.Generic;

//public class RegisterGO {
//    void Start() {
//        MyRegistry.Register(this.gameObject);
//    }

//    void OnDestroy() {
//        MyRegistry.Unregister(this.gameObject);
//    }
//}

public static class MyRegistry {
        static List<GameObject> register = new List<GameObject>();

        public static void Register(GameObject go) {
            register.Add(go);
        }

        public static void Unregister(GameObject go) {
            register.Remove(go);
        }

        public static GameObject Find(string name) {
            return register.Find(x => x.name == name);
        }
    }
