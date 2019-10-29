using Unicell.ViaCep.Modelos;

namespace Unicell.ViaCep.Net
{
    /// <summary>
    /// Define um Factory para a criação de requisições por Endereço.
    /// </summary>
    public interface IViaCepRequisicaoPorEnderecoFactory : IViaCepRequisicaoJsonDe<EnderecoRequisicao>, 
                                                           IViaCepRequisicaoXmlDe<EnderecoRequisicao>
    { }
}