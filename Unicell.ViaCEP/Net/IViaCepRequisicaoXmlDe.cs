namespace Unicell.ViaCep.Net
{
    /// <summary>
    /// Define uma requisição Xml.
    /// </summary>
    /// <typeparam name="T">O tipo do objeto contendo os dados da requisição. <see cref="Unicell.ViaCep.Modelos.Cep"/>
    /// ou <see cref="Unicell.ViaCep.Modelos.EnderecoRequisicao"/>
    /// </typeparam>
    public interface IViaCepRequisicaoXmlDe<T>
    {
        /// <summary>
        /// Cria uma nova requisição Xml.
        /// </summary>
        /// <param name="obj">O objeto contendo os dados da requisição.</param>
        /// <returns>Um objeto que representa o contexto da requisição.</returns>
        IViaCepRequisicaoPor<T> NovaRequisicaoXml(T obj);
    }
}