using UnityEngine;

namespace Project.Scripts.Field
{
    public class Sheaf : MonoBehaviour, IDestroyable
    {
        public void Destroy()
        {
            Debug.Log(name);
        }
    }
}