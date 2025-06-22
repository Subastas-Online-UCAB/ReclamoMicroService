using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reclamoService.Dominio.Excepciones
{
    public class OperacionInvalidaException : Exception
    {
        public OperacionInvalidaException(string mensaje) : base(mensaje)
        {
        }
    }
}
