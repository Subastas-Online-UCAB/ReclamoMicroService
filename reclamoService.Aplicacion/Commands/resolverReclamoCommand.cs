using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace reclamoService.Aplicacion.Commands
{
    public class resolverReclamoCommand : IRequest<bool>
    {
        public Guid ReclamoId { get; set; }

        public resolverReclamoCommand(Guid reclamoId)
        {
            ReclamoId = reclamoId;
        }
    }
}
