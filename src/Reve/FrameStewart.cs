namespace Conde.Reve;

/// <summary>
/// A conta de Frame e Stewart, de 1941: para mover <c>n</c> discos com
/// <c>p</c> hastes, escolha um corte <c>k</c>, leve os <c>k</c> menores para
/// uma haste de descanso usando todas as hastes, leve os <c>n−k</c> maiores
/// com uma haste a menos, e traga os <c>k</c> de volta.
///
/// <code>FS(n, p) = min sobre k de [ 2·FS(k, p) + FS(n−k, p−1) ]</code>
///
/// Com três hastes não há escolha e a conta vira <c>2ⁿ − 1</c>.
///
/// Que esse corte seja o melhor possível foi <b>conjectura por 73 anos</b>, e
/// só foi provado para quatro hastes em 2014. Aqui ela não é usada como
/// verdade: é comparada, disco por disco, com a varredura de todos os estados.
/// </summary>
public static class FrameStewart
{
    public static long Lances(int discos, int hastes)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(discos);
        ArgumentOutOfRangeException.ThrowIfLessThan(hastes, 3);

        var tabela = Tabela(discos, hastes);
        return tabela[hastes][discos];
    }

    private static long[][] Tabela(int discos, int hastes)
    {
        var tabela = new long[hastes + 1][];
        for (var p = 3; p <= hastes; p++)
        {
            tabela[p] = new long[discos + 1];
        }

        for (var n = 0; n <= discos; n++)
        {
            tabela[3][n] = (1L << n) - 1;
        }

        for (var p = 4; p <= hastes; p++)
        {
            tabela[p][0] = 0;
            for (var n = 1; n <= discos; n++)
            {
                var melhor = long.MaxValue;
                for (var k = 1; k <= n; k++)
                {
                    var custo = (2 * tabela[p][n - k]) + tabela[p - 1][k];
                    melhor = Math.Min(melhor, custo);
                }

                tabela[p][n] = melhor;
            }
        }

        return tabela;
    }

    /// <summary>
    /// Os cortes que alcançam o mínimo. Que às vezes sejam dois é um detalhe
    /// que o enunciado da conta não menciona, e que a tabela mostra.
    /// </summary>
    public static List<int> Cortes(int discos, int hastes)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(discos, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(hastes, 4);

        var tabela = Tabela(discos, hastes);
        var melhor = tabela[hastes][discos];
        var cortes = new List<int>();

        for (var k = 1; k <= discos; k++)
        {
            if ((2 * tabela[hastes][discos - k]) + tabela[hastes - 1][k] == melhor)
            {
                cortes.Add(k);
            }
        }

        return cortes;
    }

    /// <summary>A conta de três hastes, que não tem escolha nenhuma.</summary>
    public static long TresHastes(int discos)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(discos);
        return (1L << discos) - 1;
    }
}
