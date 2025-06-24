using System;

namespace reclamoService.Dominio.Excepciones
{
    public class reclamoNoEncontradoException : Exception
    {
        public reclamoNoEncontradoException(Guid id)
            : base($"No se encontró el reclamo con ID: {id}.")
        {
        }
    }
}