using Conde.Reve;
using Xunit;

namespace Conde.Reve.Testes;

/// <summary>
/// Os números do README presos aqui. Só entra o que eu vi o medidor imprimir.
/// </summary>
public class MedidasTestes
{
    [Fact]
    public void AVarreduraNuncaDiscordaDaContaComQuatroHastes()
    {
        foreach (var l in Medidas.Confronto(4, 10))
        {
            Assert.True(l.Concordam, $"discordaram em {l.Discos} discos");
            Assert.Equal(l.Estados, l.Alcancados);
        }
    }

    [Fact]
    public void AVarreduraNuncaDiscordaDaContaComCincoHastes()
    {
        foreach (var l in Medidas.Confronto(5, 8))
        {
            Assert.True(l.Concordam);
        }
    }

    [Fact]
    public void AVarreduraNuncaDiscordaDaContaComTresHastes()
    {
        foreach (var l in Medidas.Confronto(3, 10))
        {
            Assert.True(l.Concordam);
        }
    }

    [Theory]
    [InlineData(10, 49, 1048576)]
    [InlineData(11, 65, 4194304)]
    [InlineData(12, 81, 16777216)]
    public void AsLinhasMaioresDoConfronto(int discos, int lances, int estados)
    {
        var l = Medidas.Confronto(4, discos)[^1];
        Assert.Equal(lances, l.Varredura);
        Assert.Equal(lances, (int)l.Formula);
        Assert.Equal(estados, l.Estados);
        Assert.True(l.Concordam);
    }

    [Theory]
    [InlineData(1, 1, "1", 1)]
    [InlineData(2, 3, "1,2", 2)]
    [InlineData(3, 5, "2", 1)]
    [InlineData(6, 17, "3", 1)]
    [InlineData(10, 49, "4", 1)]
    [InlineData(15, 129, "5", 1)]
    [InlineData(21, 321, "6", 1)]
    [InlineData(28, 769, "7", 1)]
    [InlineData(20, 289, "5,6", 2)]
    [InlineData(30, 1025, "7,8", 2)]
    public void ATabelaDosCortes(int discos, long lances, string cortes, int quantos)
    {
        var l = Medidas.Cortes(discos)[^1];
        Assert.Equal(lances, l.Lances);
        Assert.Equal(cortes, l.Cortes);
        Assert.Equal(quantos, l.Quantos);
    }

    [Fact]
    public void OCorteSoEUnicoNosNumerosTriangulares()
    {
        var unicos = Medidas.Cortes(30).Where(l => l.Quantos == 1).Select(l => l.Discos).ToList();
        Assert.Equal([1, 3, 6, 10, 15, 21, 28], unicos);

        foreach (var n in unicos)
        {
            var k = 0;
            var soma = 0;
            while (soma < n)
            {
                k++;
                soma += k;
            }

            Assert.Equal(n, soma);
        }
    }

    [Fact]
    public void NosOutrosTamanhosSaoSempreDoisCortes()
    {
        foreach (var l in Medidas.Cortes(30).Where(l => l.Quantos != 1))
        {
            Assert.Equal(2, l.Quantos);
        }
    }

    [Theory]
    [InlineData(5, 31, 13, 11, 2.4)]
    [InlineData(10, 1023, 49, 31, 20.9)]
    [InlineData(20, 1048575, 289, 111, 3628.3)]
    [InlineData(30, 1073741823, 1025, 271, 1047553.0)]
    [InlineData(50, 1125899906842623, 6657, 831, 169130224852.4)]
    public void ATabelaDoGanho(int discos, long tres, long quatro, long cinco, double razao)
    {
        var l = Medidas.Ganho([discos])[0];
        Assert.Equal(tres, l.TresHastes);
        Assert.Equal(quatro, l.QuatroHastes);
        Assert.Equal(cinco, l.CincoHastes);
        Assert.Equal(razao, l.Razao);
    }

    [Theory]
    [InlineData(3, 10, 59049, 1023)]
    [InlineData(4, 8, 65536, 33)]
    [InlineData(4, 10, 1048576, 49)]
    [InlineData(4, 12, 16777216, 81)]
    [InlineData(5, 8, 390625, 23)]
    public void ATabelaDoAlcance(int hastes, int discos, int estados, int ateOFim)
    {
        var (quantos, alcancados, todos) = Medidas.Alcance(hastes, discos);
        Assert.Equal(estados, quantos);
        Assert.Equal(estados, alcancados);
        Assert.True(todos);

        var (fim, maisDistante) = Medidas.Diametro(hastes, discos);
        Assert.Equal(ateOFim, fim);
        Assert.Equal(ateOFim, maisDistante);
    }
}
