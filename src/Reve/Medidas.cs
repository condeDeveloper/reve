namespace Conde.Reve;

/// <summary>
/// As tabelas do README. Cada número sai de um método daqui e está preso num
/// teste; só entra aqui o que eu vi o programa imprimir.
/// </summary>
public static class Medidas
{
    public readonly record struct LinhaConfronto(int Discos, int Hastes, int Varredura, long Formula, bool Concordam, int Estados, int Alcancados);

    public readonly record struct LinhaCorte(int Discos, long Lances, string Cortes, int Quantos);

    public readonly record struct LinhaGanho(int Discos, long TresHastes, long QuatroHastes, long CincoHastes, double Razao);

    /// <summary>
    /// A varredura de todos os estados contra a conta de Frame e Stewart,
    /// disco por disco. É a tabela central do repositório.
    /// </summary>
    public static List<LinhaConfronto> Confronto(int hastes, int ate)
    {
        var linhas = new List<LinhaConfronto>();

        for (var n = 1; n <= ate; n++)
        {
            var r = Varredura.Minimo(hastes, n);
            var formula = FrameStewart.Lances(n, hastes);
            linhas.Add(new LinhaConfronto(n, hastes, r.Lances, formula, r.Lances == formula, r.Estados, r.Alcancados));
        }

        return linhas;
    }

    /// <summary>
    /// Os cortes que alcançam o mínimo, com quatro hastes. A pergunta é se o
    /// corte é único.
    /// </summary>
    public static List<LinhaCorte> Cortes(int ate, int hastes = 4)
    {
        var linhas = new List<LinhaCorte>();

        for (var n = 1; n <= ate; n++)
        {
            var cortes = FrameStewart.Cortes(n, hastes);
            linhas.Add(new LinhaCorte(n, FrameStewart.Lances(n, hastes), string.Join(",", cortes), cortes.Count));
        }

        return linhas;
    }

    /// <summary>O que a quarta e a quinta haste economizam.</summary>
    public static List<LinhaGanho> Ganho(IEnumerable<int> discos)
    {
        var linhas = new List<LinhaGanho>();

        foreach (var n in discos)
        {
            var tres = FrameStewart.TresHastes(n);
            var quatro = FrameStewart.Lances(n, 4);
            var cinco = FrameStewart.Lances(n, 5);
            linhas.Add(new LinhaGanho(n, tres, quatro, cinco, Math.Round((double)tres / quatro, 1)));
        }

        return linhas;
    }

    /// <summary>
    /// Quantos estados a varredura alcança, contra quantos existem. Com três
    /// hastes todo estado é alcançável; com quatro também — e saber isso não é
    /// óbvio antes de medir.
    /// </summary>
    public static (int Estados, int Alcancados, bool Todos) Alcance(int hastes, int discos)
    {
        var r = Varredura.Minimo(hastes, discos);
        return (r.Estados, r.Alcancados, r.Estados == r.Alcancados);
    }

    /// <summary>
    /// O estado mais longe do começo, contra a distância até o fim. Num
    /// problema de três hastes os dois coincidem; com quatro, não.
    /// </summary>
    public static (int AteOFim, int MaisDistante) Diametro(int hastes, int discos)
    {
        var r = Varredura.Minimo(hastes, discos);
        return (r.Lances, r.MaisDistante);
    }
}
