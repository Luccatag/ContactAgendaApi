using MediatR;
using ContactAgendaApi.DTOs;

namespace ContactAgendaApi.CQRS.Commands;

/// <summary>
/// Command to toggle contact favorite status
/// </summary>
public record ToggleFavoriteCommand(int Id) : IRequest<ContactReadDto>;
