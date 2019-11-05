using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Unicell.ViaCep;
using Unicell.ViaCep.Modelos;

namespace Unicell.WEB.Utils
{
    public class ViaCep
    {
        private IViaCepService _viaCepService;
        private EnderecoRequisicao _enderecoRequisicao;

      
        public void SetUp()
        {
            _viaCepService = ViaCepService.Default();
            _enderecoRequisicao = new EnderecoRequisicao
            {
                UF = UF.RS,
                Cidade = "Porto Alegre",
                Logradouro = "Olavo"
            };
        }

        #region ObterEnderecoComoXml

      
        public async Task DeveObterEnderecoComoXmlAsync()
        {
            var enderecoXml = await _viaCepService.ObterEnderecoComoXmlAsync("01001000");
           

            enderecoXml = await _viaCepService.ObterEnderecoComoXmlAsync("01001-000");
           

            var enderecosXml = await _viaCepService.ObterEnderecosComoXmlAsync(_enderecoRequisicao);
          
        }

    
        public void DeveObterEnderecoComoXml()
        {
            var enderecoXml = _viaCepService.ObterEnderecoComoXml("01001000");
          

            enderecoXml = _viaCepService.ObterEnderecoComoXml("01001-000");
         

            var enderecosXml = _viaCepService.ObterEnderecosComoXml(_enderecoRequisicao);
         
        }

        #endregion

        #region ObterEnderecoComoQuerty

  
        public async Task DeveObterEnderecoComoQuertyAsync()
        {
            var quertyEndereco = await _viaCepService.ObterEnderecoComoQuertyAsync("01001000");
          

            quertyEndereco = await _viaCepService.ObterEnderecoComoQuertyAsync("01001-000");
        
        }

   
        public void DeveObterEnderecoComoQuerty()
        {
            var quertyEndereco = _viaCepService.ObterEnderecoComoQuerty("01001000");
         

            quertyEndereco = _viaCepService.ObterEnderecoComoQuerty("01001-000");
         
        }

        #endregion

        #region ObterEnderecoComoPiped

    
        public async Task DeveObterEnderecoComoPipedAsync()
        {
            var pipedEndereco = await _viaCepService.ObterEnderecoComoPipedAsync("01001000");
          

            pipedEndereco = await _viaCepService.ObterEnderecoComoPipedAsync("01001-000");
          
        }

    
        public void DeveObterEnderecoComoPiped()
        {
            var pipedEndereco = _viaCepService.ObterEnderecoComoPiped("01001000");
            

            pipedEndereco = _viaCepService.ObterEnderecoComoPiped("01001-000");
         
        }

        #endregion

        #region ObterEnderecoComoJson

  
        public async Task DeveObterEnderecoComoJsonAsync()
        {
            var jsonEndereco = await _viaCepService.ObterEnderecoComoJsonAsync("01001000");
        

            jsonEndereco = await _viaCepService.ObterEnderecoComoJsonAsync("01001-000");
        

            var jsonEnderecos = await _viaCepService.ObterEnderecosComoJsonAsync(_enderecoRequisicao);
         
        }

     
        public void DeveObterEnderecoComoJson()
        {
            var jsonEndereco = _viaCepService.ObterEnderecoComoJson("01001000");
     

            jsonEndereco = _viaCepService.ObterEnderecoComoJson("01001-000");
           

            var jsonEnderecos = _viaCepService.ObterEnderecosComoJson(_enderecoRequisicao);
          
        }

        #endregion

        #region ObterEndereco

   
        public async Task DeveObterEnderecoAsync()
        {
            var endereco = await _viaCepService.ObterEnderecoAsync("01001000");
          

            endereco = await _viaCepService.ObterEnderecoAsync("01001-000");
        

            var enderecos = await _viaCepService.ObterEnderecosAsync(_enderecoRequisicao);
          
        }

      
        public void DeveObterEndereco()
        {
            var endereco = _viaCepService.ObterEndereco("01001000");
        

            endereco = _viaCepService.ObterEndereco("01001-000");
            

            var enderecos = _viaCepService.ObterEnderecos(_enderecoRequisicao);
         
        }

        #endregion

        public void TearDown() => _viaCepService.Dispose();
    }
}