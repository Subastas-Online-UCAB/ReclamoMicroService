using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using reclamoService.Aplicacion.DTOs;

namespace reclamoService.Aplicacion.Queries
{
    public class ObtenerTodosLosReclamosQuery : IRequest<List<ReclamoDto>> { }
}
