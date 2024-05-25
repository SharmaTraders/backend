using System.ComponentModel.DataAnnotations;
using CommandContracts.employee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.employee;

public class UpdateSalary : CommandEndPointBase
    .WithRequest<UpdateSalaryRequest>
    .WithoutResponse {

    private readonly IMediator _mediator;

    public UpdateSalary(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPatch, Route("employee/{id}/salary")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult> HandleAsync(UpdateSalaryRequest request) {
        var commandRequest = new UpdateSalaryCommand.Request(
            request.Id,
            request.RequestBody.StartDate,
            request.RequestBody.SalaryPerHour,
            request.RequestBody.OvertimeSalaryPerHour
        );
        await _mediator.Send(commandRequest);
        return Ok();
    }
}

public class UpdateSalaryRequest {
    [FromRoute] public string Id { get; set; } = null!;
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(
        string StartDate,
        [Required]
        double SalaryPerHour,

        [Required]
        double OvertimeSalaryPerHour
    );
}