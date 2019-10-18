using System;
using System.Collections.Generic;
using System.Text;

namespace Unicell.DTO
{
    public class MasterDTO<TGrid>
    {
        public int? draw { get; set; }
        public int? recordsTotal { get { return (sucesso != null) ? sucesso.MaxRows : 0; } }
        public int? recordsFiltered { get { return (sucesso != null) ? sucesso.MaxRows : 0; } }
        public List<TGrid> data { get; set; }

        private SucessoDTO _sucesso;

        public SucessoDTO sucesso
        {
            get { return _sucesso; }
            set
            {
                if (value.sucesso.HasValue && value.sucesso.Value)
                    _sucesso = value;
                else
                    throw new Exception(value.mensagem);

            }
        }
    }
}
