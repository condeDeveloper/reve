namespace Conde.Reve;

/// <summary>
/// Um estado da torre: em que haste está cada disco.
///
/// Os discos são numerados do menor (0) para o maior, e dentro de uma haste a
/// ordem está determinada — disco maior nunca fica em cima de menor, e é essa
/// a única regra do jogo. Então o estado inteiro cabe num número na base
/// <c>hastes</c>, com um algarismo por disco: com 4 hastes e 13 discos são
/// 67.108.864 estados, que cabem num vetor de bytes.
///
/// Essa codificação é o que torna a varredura possível. Guardar pilhas de
/// verdade custaria memória e, pior, exigiria normalizar para comparar.
/// </summary>
public static class Estado
{
    public static int Quantidade(int hastes, int discos)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(hastes, 1);
        ArgumentOutOfRangeException.ThrowIfNegative(discos);

        var total = 1;
        for (var i = 0; i < discos; i++)
        {
            total *= hastes;
        }

        return total;
    }

    public static int HasteDo(int estado, int disco, int hastes)
    {
        for (var i = 0; i < disco; i++)
        {
            estado /= hastes;
        }

        return estado % hastes;
    }

    /// <summary>
    /// O menor disco de cada haste, ou <c>-1</c> quando a haste está vazia. É a
    /// única informação de que um lance precisa.
    /// </summary>
    public static int[] Topos(int estado, int hastes, int discos)
    {
        var topos = new int[hastes];
        Array.Fill(topos, -1);

        // Dos discos maiores para os menores: o que ficar por ultimo em cada
        // haste e o menor, que e o que esta por cima.
        for (var disco = discos - 1; disco >= 0; disco--)
        {
            topos[HasteDo(estado, disco, hastes)] = disco;
        }

        return topos;
    }

    public static int TudoNa(int haste, int hastes, int discos)
    {
        var estado = 0;
        var peso = 1;
        for (var i = 0; i < discos; i++)
        {
            estado += haste * peso;
            peso *= hastes;
        }

        return estado;
    }

    /// <summary>O estado depois de mover um disco para outra haste.</summary>
    public static int Mover(int estado, int disco, int destino, int hastes)
    {
        var peso = 1;
        for (var i = 0; i < disco; i++)
        {
            peso *= hastes;
        }

        var atual = (estado / peso) % hastes;
        return estado + ((destino - atual) * peso);
    }

    public static string Texto(int estado, int hastes, int discos)
    {
        var pilhas = new List<int>[hastes];
        for (var h = 0; h < hastes; h++)
        {
            pilhas[h] = [];
        }

        for (var disco = discos - 1; disco >= 0; disco--)
        {
            pilhas[HasteDo(estado, disco, hastes)].Add(disco);
        }

        return string.Join(" | ", pilhas.Select(p => string.Join(",", p)));
    }
}
