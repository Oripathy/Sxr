using System;
using System.Collections;
using UnityEngine;

namespace View
{
    internal class UpdateHandler : MonoBehaviour
    {
        public event Action UpdateTicked;

        public void Update()
        {
            UpdateTicked?.Invoke();
        }

        public void ExecuteCoroutine(IEnumerator coroutine) => StartCoroutine(coroutine);
    }
}