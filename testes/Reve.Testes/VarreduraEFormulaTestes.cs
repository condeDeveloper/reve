using Conde.Reve;
using Xunit;

namespace Conde.Reve.Testes;

public class VarreduraEFormulaTestes
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 7)]
    [InlineData(5, 31)]
    [InlineData(10, 1023)]
    public void ComTresHastesAVarreduraDaDoisElevadoANMenosUm(int discos, int esperado)
    {
        Assert.Equal(esperado, Varredura.Minimo(3, discos).Lances);
        Assert.Equal(esperado, FrameStewart.TresHastes(discos));
        Assert.Equal(esperado, FrameStewart.Lances(discos, 3));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 5)]
    [InlineData(4, 9)]
    [InlineData(5, 13)]
    [InlineData(6, 17)]
    [InlineData(7, 25)]
    [InlineData(8, 33)]
    [InlineData(9, 41)]
    [InlineData(10, 49)]
    public void ComQuatroHastesAVarreduraConcordaComFrameEStewart(int discos, int esperado)
    {
        Assert.Equal(esperado, Varredura.Minimo(4, discos).Lances);
        Assert.Equal(esperado, FrameStewart.Lances(discos, 4));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(4, 7)]
    [InlineData(5, 11)]
    [InlineData(8, 23)]
    [InlineData(9, 27)]
    public void ComCincoHastesTambemConcordam(int discos, int esperado)
    {
        Assert.Equal(esperado, Varredura.Minimo(5, discos).Lances);
        Assert.Equal(esperado, FrameStewart.Lances(discos, 5));
    }

    [Fact]
    public void AQuartaHasteNuncaPiora()
    {
        for (var n = 1; n <= 20; n++)
        {
            Assert.True(FrameStewart.Lances(n, 4) <= FrameStewart.TresHastes(n));
            Assert.True(FrameStewart.Lances(n, 5) <= FrameStewart.Lances(n, 4));
        }
    }

    [Fact]
    public void TodoEstadoEAlcancavel()
    {
        foreach (var (hastes, discos) in new[] { (3, 8), (4, 8), (5, 6) })
        {
            var r = Varredura.Minimo(hastes, discos);
            Assert.Equal(r.Estados, r.Alcancados);
        }
    }

    [Fact]
    public void OEstadoMaisDistanteDoComecoEOProprioFim()
    {
        foreach (var (hastes, discos) in new[] { (3, 8), (4, 8), (4, 10), (5, 6) })
        {
            var r = Varredura.Minimo(hastes, discos);
            Assert.Equal(r.Lances, r.MaisDistante);
        }
    }

    [Fact]
    public void ZeroDiscoNaoCustaLanceNenhum()
    {
        Assert.Equal(0, Varredura.Minimo(4, 0).Lances);
        Assert.Equal(0, FrameStewart.Lances(0, 4));
    }

    [Fact]
    public void OsCortesOtimosSaoRealmenteOtimos()
    {
        for (var n = 1; n <= 20; n++)
        {
            var melhor = FrameStewart.Lances(n, 4);
            foreach (var k in FrameStewart.Cortes(n, 4))
            {
                var custo = (2 * FrameStewart.Lances(n - k, 4)) + FrameStewart.TresHastes(k);
                Assert.Equal(melhor, custo);
            }
        }
    }

    [Fact]
    public void NenhumCorteBateOMinimo()
    {
        for (var n = 1; n <= 20; n++)
        {
            var melhor = FrameStewart.Lances(n, 4);
            for (var k = 1; k <= n; k++)
            {
                var custo = (2 * FrameStewart.Lances(n - k, 4)) + FrameStewart.TresHastes(k);
                Assert.True(custo >= melhor);
            }
        }
    }

    [Fact]
    public void ArgumentosInvalidosNaoSaoAceitos()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Varredura.Minimo(2, 3));
        Assert.Throws<ArgumentOutOfRangeException>(() => Varredura.Minimo(4, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => FrameStewart.Lances(-1, 4));
        Assert.Throws<ArgumentOutOfRangeException>(() => FrameStewart.Lances(5, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => FrameStewart.Cortes(0, 4));
        Assert.Throws<ArgumentOutOfRangeException>(() => FrameStewart.Cortes(5, 3));
    }
}
