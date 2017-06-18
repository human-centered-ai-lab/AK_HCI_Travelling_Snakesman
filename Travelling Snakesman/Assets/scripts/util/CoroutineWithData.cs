using System.Collections;
using UnityEngine;

namespace util
{
    public class CoroutineWithData<T>
    {
        private readonly IEnumerator _target;
        public T Result;
        public Coroutine Coroutine { get; private set; }

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            _target = target;
            Coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (_target.MoveNext())
            {
                Result = (T)_target.Current;
                yield return Result;
            }
        }
    }
}
