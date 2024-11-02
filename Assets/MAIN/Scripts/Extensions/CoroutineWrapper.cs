using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Uma classe wrapper para gerenciar corrotinas no Unity.
/// </summary>
public class CoroutineWrapper
{
    /// <summary>
    /// O MonoBehaviour que possui a corrotina.
    /// </summary>
    private MonoBehaviour owner;

    /// <summary>
    /// A corrotina sendo gerenciada.
    /// </summary>
    private Coroutine coroutine;

    /// <summary>
    /// Indica se a corrotina foi concluída.
    /// </summary>
    public bool IsDone = false;

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="CoroutineWrapper"/>.
    /// </summary>
    /// <param name="owner">O MonoBehaviour que possui a corrotina.</param>
    /// <param name="coroutine">A corrotina a ser gerenciada.</param>
    public CoroutineWrapper(MonoBehaviour owner, Coroutine coroutine)
    {
        this.owner = owner;
        this.coroutine = coroutine;
    }

    /// <summary>
    /// Para a corrotina e marca como concluída.
    /// </summary>
    public void Stop()
    {
        owner.StopCoroutine(coroutine);
        IsDone = true;
    }
}
