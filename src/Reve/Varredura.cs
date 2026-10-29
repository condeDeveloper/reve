namespace Conde.Reve;

/// <summary>
/// O juiz: a busca em largura sobre TODOS os estados, que devolve o número
/// mínimo de lances sem saber nada sobre torres de Hanói.
///
/// Ela não tem fórmula, não tem recursão e não tem palpite sobre como dividir
/// a pilha. É a resposta de referência contra a qual a conta de Frame e Stewart
/// é conferida — e com quatro hastes essa conta foi conjectura de 1941 até
/// 2014, o que torna a comparação mais interessante do que de costume.
/// </summary>
public static class Varredura
{
    public readonly record struct Resultado(int Lances, int Estados, int Alcancados, int MaisDistante);

    public static Resultado Minimo(int hastes, int discos)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(hastes, 3);
        ArgumentOutOfRangeException.ThrowIfNegative(discos);

        var total = Estado.Quantidade(hastes, discos);
        var distancia = new int[total];
        Array.Fill(distancia, -1);

        var inicio = Estado.TudoNa(0, hastes, discos);
        var fim = Estado.TudoNa(hastes - 1, hastes, discos);

        distancia[inicio] = 0;
        var fila = new Queue<int>();
        fila.Enqueue(inicio);
        var alcancados = 1;
        var maisDistante = 0;

        while (fila.Count > 0)
        {
            var atual = fila.Dequeue();
            var topos = Estado.Topos(atual, hastes, discos);

            for (var origem = 0; origem < hastes; origem++)
            {
                var disco = topos[origem];
                if (disco < 0)
                {
                    continue;
                }

                for (var destino = 0; destino < hastes; destino++)
                {
                    if (destino == origem)
                    {
                        continue;
                    }

                    // So pode pousar em haste vazia ou em cima de disco maior.
                    if (topos[destino] >= 0 && topos[destino] < disco)
                    {
                        continue;
                    }

                    var seguinte = Estado.Mover(atual, disco, destino, hastes);
                    if (distancia[seguinte] >= 0)
                    {
                        continue;
                    }

                    distancia[seguinte] = distancia[atual] + 1;
                    maisDistante = Math.Max(maisDistante, distancia[seguinte]);
                    alcancados++;
                    fila.Enqueue(seguinte);
                }
            }
        }

        return new Resultado(distancia[fim], total, alcancados, maisDistante);
    }
}
