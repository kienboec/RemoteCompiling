using System.ComponentModel.DataAnnotations;
using MediatR;
using RestWebservice_RemoteCompiling.Entities;
using RestWebservice_RemoteCompiling.JsonObjClasses;

namespace RestWebservice_RemoteCompiling.Command
{
    public class ExecuteCodeCommand : IRequest<CustomResponse<PistonCompileAndRun>>
    {
        public string Language { get; set; }
        public string Version { get; set; }
        public Code Code { get; set; }
    }
}