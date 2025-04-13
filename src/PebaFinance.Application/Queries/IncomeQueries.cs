using MediatR;
using PebaFinance.Application.DTOs;

namespace PebaFinance.Application.Queries;

public record GetIncomeByIdQuery(int Id) : IRequest<IncomeDto?>;
public record GetAllIncomesQuery : IRequest<IEnumerable<IncomeDto>>;