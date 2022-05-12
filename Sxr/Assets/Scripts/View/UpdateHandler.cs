using System;
using System.Collections;
using UnityEngine;

namespace View
{
    internal class UpdateHandler : MonoBehaviour
    {
        public event Action<float> UpdateTicked;

        public void Update()
        {
            UpdateTicked?.Invoke(Time.deltaTime);
        }

        public void ExecuteCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);
    }
}