using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace reclamoService.Aplicacion.Commands
{
    public record AgregarSolucionReclamoCommand(Guid ReclamoId, string Solucion) : IRequest<Unit>;
}
